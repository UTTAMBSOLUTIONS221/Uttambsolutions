﻿CREATE TABLE [dbo].[Systempropertyhouseroommeters] (
    [Systempropertyhousemeterid]         INT             IDENTITY (1, 1) NOT NULL,
    [Systempropertyhouseroomid]          INT             NOT NULL,
    [Systempropertyhouseroommeternumber] VARCHAR (100)   NOT NULL,
    [Periodid]                           INT             NOT NULL,
    [Openingmeter]                       DECIMAL (18, 2) NOT NULL,
    [Movedmeter]                         DECIMAL (18, 2) NOT NULL,
    [Closingmeter]                       DECIMAL (18, 2) NOT NULL,
    [Consumedamount]                     DECIMAL (18, 2) NOT NULL,
    [Createdby]                          INT             NOT NULL,
    [Datecreated]                        DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([Systempropertyhousemeterid] ASC)
);


GO
CREATE TRIGGER [dbo].[trg_AfterInsert_Generatesystemtenantsubsequentinvoicebill]
ON [dbo].[Systempropertyhouseroommeters]
AFTER INSERT, UPDATE
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '',
            @FinanceTransactionId BIGINT,
            @Ticketid BIGINT,
            @Invoiceid BIGINT;

    BEGIN TRY
        -- Start transaction
        BEGIN TRANSACTION;

        DECLARE @SystemfinanceTransactiondata TABLE (FinanceTransactionId BIGINT);
        DECLARE @Systemticketdata TABLE (Ticketid BIGINT);
        DECLARE @SystemInvoiceiddata TABLE (Invoiceid BIGINT);

        -- Temporary table to hold merged data
        CREATE TABLE #MergedData (
            Systempropertyhouseroomid INT,
            Systempropertyhousetenantid INT,
            Systempropertyhouseid INT,
            Accountid INT,
            Accountnumber VARCHAR(50),
            Rentdueday INT,
			Periodid INT,
			Invoicestatus VARCHAR(20),
            Ticketlines NVARCHAR(MAX),
            TotalAmount DECIMAL(18, 2),
        );
      -- Insert merged data from the inserted or updated rows and other relevant tables
        INSERT INTO #MergedData (Systempropertyhouseroomid, Systempropertyhousetenantid, Systempropertyhouseid, Accountid, Accountnumber, Rentdueday,Periodid,Invoicestatus, Ticketlines, TotalAmount)
         SELECT i.Systempropertyhouseroomid,tenant.Systempropertyhousetenantid,room.Systempropertyhouseid,account.Accountid,account.Accountnumber,Propertyhouse.Rentdueday,(SELECT x.PeriodId FROM Systemperiods x WHERE x.Lastdateinperiod=(SELECT EOMONTH(GETDATE(), 0))) AS Periodid,'Newinvoice' AS Invoicestatus,
		(
				-- Fetch non-recurring ticket lines as JSON, including house rent
				SELECT deposit.Systempropertyhousedepositfeeid,housedeposit.Housedepositfeename AS Name,
					CASE 
						WHEN housedeposit.Housedepositfeename IN ('Water Deposit', 'Electricity Deposit', 'House Rent', 'Bin Fee', 'Security Fee') THEN 1 
						WHEN housedeposit.Housedepositfeename = 'Water Bill' THEN 0 
						ELSE 1 
					END AS Units,
					CASE 
						WHEN housedeposit.Housedepositfeename IN ('Water Deposit', 'Electricity Deposit', 'Bin Fee', 'Security Fee') THEN deposit.Systempropertyhousedepositfeeamount 
						WHEN housedeposit.Housedepositfeename = 'House Rent' THEN room.Systempropertyhousesizerent 
						WHEN housedeposit.Housedepositfeename = 'Water Bill' THEN 0
						ELSE 0 
					END AS Amount,
					0 AS Discount,
					0 AS Createdby,
					GETDATE() AS Datecreated,
					housedeposit.Isrecurring
				FROM Systempropertyhousedepositfees deposit 
				INNER JOIN Systemhousedepositfees housedeposit ON deposit.Housedepositfeeid = housedeposit.Housedepositfeeid 
				INNER JOIN Systempropertyhousesizes rental ON deposit.Propertyhouseid = rental.Propertyhouseid
				INNER JOIN  Systempropertyhouserooms room ON rental.Systempropertyhousesizeid = room.Systempropertyhousesizeid
				INNER JOIN Systempropertyhouses  Propertyhouse ON room.Systempropertyhouseid=Propertyhouse.Propertyhouseid
				WHERE room.Systempropertyhouseroomid = i.Systempropertyhouseroomid AND housedeposit.Isrecurring = 1 
				FOR JSON PATH
			) AS NonRecurringTicketlines,
			(
				-- Calculate the total amount for non-recurring items, adding house rent
				SELECT SUM(Amount) AS NonRecurringTotal
				FROM (
					SELECT 
						CASE 
							WHEN housedeposit.Housedepositfeename = 'House Deposit' AND Propertyhouse.Hashousedeposit = 1  THEN room.Systempropertyhousesizerent 
							WHEN housedeposit.Housedepositfeename IN ('Water Deposit', 'Electricity Deposit', 'Bin Fee', 'Security Fee') THEN deposit.Systempropertyhousedepositfeeamount 
							WHEN housedeposit.Housedepositfeename = 'House Rent' THEN room.Systempropertyhousesizerent 
							WHEN housedeposit.Housedepositfeename = 'Water Bill' THEN 0
							ELSE 0 
						END AS Amount
					FROM  Systempropertyhousedepositfees deposit 
					INNER JOIN  Systemhousedepositfees housedeposit ON deposit.Housedepositfeeid = housedeposit.Housedepositfeeid 
					INNER JOIN Systempropertyhousesizes rental ON deposit.Propertyhouseid = rental.Propertyhouseid
					INNER JOIN   Systempropertyhouserooms room ON rental.Systempropertyhousesizeid = room.Systempropertyhousesizeid 
					INNER JOIN Systempropertyhouses  Propertyhouse ON room.Systempropertyhouseid=Propertyhouse.Propertyhouseid
					WHERE room.Systempropertyhouseroomid = i.Systempropertyhouseroomid  AND housedeposit.Isrecurring = 1
				) AS NonRecurringAmountsSubquery
			) AS NonRecurringTotalAmount
		FROM inserted i
		INNER JOIN   Systempropertyhouserooms room ON i.Systempropertyhouseroomid = room.Systempropertyhouseroomid
		INNER JOIN   Systempropertyhouseroomstenant tenant ON room.Systempropertyhouseroomid = tenant.Systempropertyhouseroomid
		INNER JOIN Systempropertyhousesizes rental ON room.Systempropertyhousesizeid = rental.Systempropertyhousesizeid
		INNER JOIN Systempropertyhouses Propertyhouse ON room.Systempropertyhouseid=Propertyhouse.Propertyhouseid
		INNER JOIN Systemstaffsaccount account ON tenant.Systempropertyhousetenantid = account.Userid;
        -- Loop through the data and insert or update records in Monthlyrentinvoices and Monthlyrentinvoiceitems
        DECLARE @RoomId INT, @TenantId INT, @TotalAmount DECIMAL(18,2), @RentDueDay INT,@Periodid INT, @Ticketlines NVARCHAR(MAX), @AccountId INT,@Invoicestatus VARCHAR(20);
        SELECT @RoomId=Systempropertyhouseroomid, @TenantId=Systempropertyhousetenantid, @TotalAmount=TotalAmount, @RentDueDay=Rentdueday,@Periodid=Periodid, @Ticketlines=Ticketlines, @AccountId=Accountid,@Invoicestatus=Invoicestatus
        FROM #MergedData;
		IF EXISTS (SELECT 1 FROM Monthlyrentinvoices WHERE Propertyhouseroomid = @RoomId AND Propertyhouseroomtenantid = @TenantId AND Ispaid=0 AND Invoicestatus='Newinvoice' AND Periodid =(SELECT x.PeriodId FROM Systemperiods x WHERE x.Lastdateinperiod=(SELECT EOMONTH(GETDATE(), 0))))
            BEGIN
                -- Update the existing invoice
				-- Merge into Monthlyrentinvoices
				SELECT @Invoiceid=Invoiceid FROM Monthlyrentinvoices WHERE Propertyhouseroomid = @RoomId AND Propertyhouseroomtenantid = @TenantId AND Periodid =(SELECT x.PeriodId FROM Systemperiods x WHERE x.Lastdateinperiod=(SELECT EOMONTH(GETDATE(), 0)))
				UPDATE Monthlyrentinvoices SET Amount = @TotalAmount,Balance = @TotalAmount WHERE Propertyhouseroomtenantid=@TenantId AND Propertyhouseroomid=@RoomId AND Periodid= @Periodid

                -- Merge into Monthlyrentinvoiceitems
				MERGE INTO Monthlyrentinvoiceitems AS target
				USING (
				SELECT @Invoiceid AS Invoiceid,JSONData.Systempropertyhousedepositfeeid,JSONData.Units,JSONData.Amount,JSONData.Discount FROM #MergedData source CROSS APPLY OPENJSON(source.Ticketlines)
				WITH (Systempropertyhousedepositfeeid BIGINT '$.Systempropertyhousedepositfeeid',Units DECIMAL(18, 2) '$.Units',Amount DECIMAL(18, 2) '$.Amount', Discount DECIMAL(18, 2) '$.Discount'
				) AS JSONData
				) AS source
				ON target.Invoiceid = source.Invoiceid
				AND target.Systempropertyhousedepositfeeid = source.Systempropertyhousedepositfeeid
				WHEN MATCHED THEN
				UPDATE SET target.Units = source.Units,target.Price = source.Amount,target.Discount = source.Discount
				WHEN NOT MATCHED THEN
				INSERT (Invoiceid, Systempropertyhousedepositfeeid, Units, Price, Discount)
				VALUES (source.Invoiceid, source.Systempropertyhousedepositfeeid, source.Units, source.Amount, source.Discount);
		    END
            ELSE
            BEGIN
			
                INSERT INTO FinanceTransactions(Tenantid,TransactionCode,FinanceTransactionTypeId,FinanceTransactionSubTypeId,ParentId,Saledescription,IsOnlineSale,Createdby,ActualDate,DateCreated)
				OUTPUT inserted.FinanceTransactionId INTO @SystemfinanceTransactiondata
				SELECT Systempropertyhousetenantid,'TXN'+''+CONVERT(VARCHAR(70),NEXT VALUE FOR TransactionCodeSequence),(SELECT TOP 1 FinanceTransactionTypeId FROM FinanceTransactionTypes WHERE FinanceTransactionType='Monthly Rent'),(SELECT TOP 1 FinanceTransactionSubTypeId FROM FinanceTransactionSubTypes WHERE FinanceTransactionSubType='Bill Generattion'),0, 'New Tenant House Rent, House Deposits and Other Deposit', 1, 0, GETDATE(),GETDATE() FROM #MergedData

				SET @FinanceTransactionId = (SELECT TOP 1 FinanceTransactionId FROM @SystemfinanceTransactiondata);

				--inserting to Gl Transactions
				INSERT INTO GLTransactions(FinanceTransactionId,ChartofAccountId,PeriodId,Amount,GlActualDate,DateCreated)
				SELECT @FinanceTransactionId,(SELECT ChartofAccountId FROM ChartofAccounts WHERE ChartofAccountname = Accountnumber),1,TotalAmount,GETDATE(),GETDATE() FROM #MergedData;

				INSERT INTO GLTransactions(FinanceTransactionId,ChartofAccountId,PeriodId,Amount,GlActualDate,DateCreated)
				SELECT @FinanceTransactionId,(SELECT ChartofAccountId FROM ChartofAccounts WHERE ChartofAccountname='Accounts Payable'),1,TotalAmount,GETDATE(),GETDATE() FROM #MergedData;

				--inserting to finance transaction 
				INSERT INTO SystemTickets(FinanceTransactionId,HouseRoomId,AccountId,Createdby,ActualDate,DateCreated)
				OUTPUT inserted.Ticketid INTO @Systemticketdata
				SELECT @FinanceTransactionId,Systempropertyhouseroomid,Accountid,0,GETDATE(),GETDATE() FROM #MergedData

				SET @Ticketid = (SELECT TOP 1 Ticketid FROM @Systemticketdata);

				--inserting to Ticket Lines
				INSERT INTO Ticketlines (TicketId,Systempropertyhousedepositfeeid,Units,Price,Discount,Createdby,DateCreated)
				SELECT @TicketId, Systempropertyhousedepositfeeid,Units,Amount,Discount, Createdby,DateCreated
				FROM #MergedData CROSS APPLY OPENJSON(Ticketlines) WITH (Systempropertyhousedepositfeeid BIGINT '$.Systempropertyhousedepositfeeid',Units DECIMAL(18,2) '$.Units', Amount DECIMAL(18,2) '$.Amount',Discount DECIMAL(18,2) '$.Discount',Createdby BIGINT '$.Createdby',Datecreated DATETIME2(6) '$.Datecreated');

				INSERT INTO Monthlyrentinvoices(Invoiceno,Propertyhouseroomid,Propertyhouseroomtenantid,Financetransactionid,Periodid,Invoicestatus,Datecreated,Duedate,Amount,Discount,Ispaid,Paidamount,Balance,Issent,Paidstatus)
				OUTPUT inserted.Invoiceid INTO @SystemInvoiceiddata
				SELECT 'INV'+''+CONVERT(VARCHAR(7),NEXT VALUE FOR InvoiceTransactionCodeSequence),Systempropertyhouseroomid,Systempropertyhousetenantid,@FinanceTransactionId,(SELECT x.PeriodId FROM Systemperiods x WHERE x.Lastdateinperiod=(SELECT EOMONTH(GETDATE(), 0))),Invoicestatus,GETDATE(),GETDATE(),TotalAmount,0,0,0,TotalAmount,0,'NOT PAID' FROM #MergedData;
				SET @Invoiceid = (SELECT TOP 1 Invoiceid FROM @SystemInvoiceiddata);

				INSERT INTO Monthlyrentinvoiceitems(Invoiceid,Systempropertyhousedepositfeeid,Units,Price,Discount)
				SELECT @Invoiceid, Systempropertyhousedepositfeeid,Units,Amount,Discount
				FROM #MergedData CROSS APPLY OPENJSON(Ticketlines) WITH (Systempropertyhousedepositfeeid BIGINT '$.Systempropertyhousedepositfeeid',Units DECIMAL(18,2) '$.Units', Amount DECIMAL(18,2) '$.Amount',Discount DECIMAL(18,2) '$.Discount',Createdby BIGINT '$.Createdby',Datecreated DATETIME2(6) '$.Datecreated');
				
				UPDATE Systempropertyhouseroomstenant SET Isnewtenant=0 WHERE Systempropertyhousetenantid =(SELECT TOP 1 Systempropertyhousetenantid FROM #MergedData);
           DROP TABLE #MergedData;
		   END

        -- Commit the transaction
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Rollback if any error occurs
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH;
END;