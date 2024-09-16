CREATE TABLE [dbo].[Systempropertyhouseroomstenant] (
    [Systempropertyhousetenantentryid] BIGINT          IDENTITY (1, 1) NOT NULL,
    [Systempropertyhousetenantid]      BIGINT          NOT NULL,
    [Systempropertyhouseroomid]        INT             NOT NULL,
    [Isoccupant]                       BIT             DEFAULT ((0)) NOT NULL,
    [Occupationalstatus]               INT             NOT NULL,
    [Roomoccupant]                     INT             DEFAULT ((0)) NOT NULL,
    [Roomoccupantdetail]               VARCHAR (200)   NOT NULL,
    [Isnewtenant]                      BIT             DEFAULT ((0)) NOT NULL,
    [Ratingvalue]                      DECIMAL (10, 2) DEFAULT ((0)) NOT NULL,
    [Ownercomment]                     VARCHAR (200)   DEFAULT ('Not Set') NOT NULL,
    [Createdby]                        INT             NOT NULL,
    [Modifiedby]                       INT             NOT NULL,
    [Vacateddate]                      DATETIME        NOT NULL,
    [Datecreated]                      DATETIME        NOT NULL,
    [Datemodified]                     DATETIME        NOT NULL,
    [Lastinvoiceddate]                 DATETIME        DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Systempropertyhousetenantentryid] ASC)
);












GO
CREATE TRIGGER [dbo].[trg_AfterInsert_Generatesystemtenantinvoicebillfornewtenant]
ON [dbo].[Systempropertyhouseroomstenant]
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
            Ticketlines NVARCHAR(MAX),
            TotalAmount DECIMAL(18, 2)
        );

        -- Insert merged data from the inserted or updated rows and other relevant tables
        INSERT INTO #MergedData (Systempropertyhouseroomid, Systempropertyhousetenantid, Systempropertyhouseid, Accountid, Accountnumber, Rentdueday,Periodid, Ticketlines, TotalAmount)
        SELECT 
            i.Systempropertyhouseroomid, 
            i.Systempropertyhousetenantid, 
            room.Systempropertyhouseid, 
            account.Accountid, 
            account.Accountnumber, 
            Propertyhouse.Rentdueday,
			(SELECT x.PeriodId FROM Systemperiods x WHERE x.Lastdateinperiod=(SELECT EOMONTH(GETDATE(), 0))) AS Periodid,
            (
                -- Fetch ticket lines as JSON, including house rent
               SELECT deposit.Systempropertyhousedepositfeeid,housedeposit.Housedepositfeename AS Name,
					CASE 
						WHEN housedeposit.Housedepositfeename IN ('House Deposit', 'Water Deposit', 'Electricity Deposit', 'House Rent', 'Bin Fee', 'Security Fee') THEN 1 
						WHEN housedeposit.Housedepositfeename = 'Water Bill' THEN 0 
						ELSE 1 
					END AS Units,
					CASE 
						WHEN housedeposit.Housedepositfeename = 'House Deposit' AND Propertyhouse.Hashousedeposit = 1 THEN room.Systempropertyhousesizerent 
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
                INNER JOIN Systempropertyhouserooms room ON deposit.Propertyhouseid = room.Systempropertyhouseid
                WHERE room.Systempropertyhouseroomid = i.Systempropertyhouseroomid
                FOR JSON PATH
            ) AS Ticketlines,
            (
                -- Calculate total amount for the ticket lines
                SELECT 
                    CASE 
							WHEN housedeposit.Housedepositfeename = 'House Deposit' AND Propertyhouse.Hashousedeposit = 1  THEN room.Systempropertyhousesizerent 
							WHEN housedeposit.Housedepositfeename IN ('Water Deposit', 'Electricity Deposit', 'Bin Fee', 'Security Fee') THEN deposit.Systempropertyhousedepositfeeamount 
							WHEN housedeposit.Housedepositfeename = 'House Rent' THEN room.Systempropertyhousesizerent 
							WHEN housedeposit.Housedepositfeename = 'Water Bill' THEN 0
							ELSE 0 
						END 
                FROM Systempropertyhousedepositfees deposit 
                INNER JOIN Systemhousedepositfees housedeposit ON deposit.Housedepositfeeid = housedeposit.Housedepositfeeid 
                INNER JOIN Systempropertyhouserooms room ON deposit.Propertyhouseid = room.Systempropertyhouseid
                WHERE room.Systempropertyhouseroomid = i.Systempropertyhouseroomid
            ) AS TotalAmount
        FROM inserted i
        INNER JOIN Systempropertyhouserooms room ON i.Systempropertyhouseroomid = room.Systempropertyhouseroomid
        INNER JOIN Systemstaffsaccount account ON i.Systempropertyhousetenantid = account.Userid
        INNER JOIN Systempropertyhouses Propertyhouse ON room.Systempropertyhouseid = Propertyhouse.Propertyhouseid;

        -- Loop through the data and insert or update records in Monthlyrentinvoices and Monthlyrentinvoiceitems
        DECLARE @RoomId INT, @TenantId INT, @TotalAmount DECIMAL(18,2), @RentDueDay INT,@Periodid INT, @Ticketlines NVARCHAR(MAX), @AccountId INT;

        DECLARE Cur CURSOR FOR 
        SELECT Systempropertyhouseroomid, Systempropertyhousetenantid, TotalAmount, Rentdueday,Periodid, Ticketlines, Accountid
        FROM #MergedData;

        OPEN Cur;
        FETCH NEXT FROM Cur INTO @RoomId, @TenantId, @TotalAmount, @RentDueDay,@Periodid, @Ticketlines, @AccountId;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Check if the invoice already exists
            IF EXISTS (SELECT 1 FROM Monthlyrentinvoices WHERE Propertyhouseroomid = @RoomId AND Propertyhouseroomtenantid = @TenantId)
            BEGIN
                -- Update the existing invoice
				-- Merge into Monthlyrentinvoices
				MERGE INTO Monthlyrentinvoices AS target USING #MergedData AS source ON target.Propertyhouseroomid = source.Systempropertyhouseroomid AND target.Propertyhouseroomtenantid = source.Systempropertyhousetenantid
				WHEN MATCHED THEN 
				UPDATE SET target.Amount = source.TotalAmount,target.Balance = source.TotalAmount,target.Duedate = GETDATE(),target.Paidamount = 0,target.Ispaid = 0, target.Paidstatus = 'NOT PAID',target.Issent = 0,target.Discount = 0
				WHEN NOT MATCHED THEN 
				INSERT (Invoiceno, Propertyhouseroomid, Propertyhouseroomtenantid, Financetransactionid,PeriodId, Datecreated, Duedate, Amount, Discount, Ispaid, Paidamount, Balance, Issent, Paidstatus)
				VALUES ('INV' + '' + CONVERT(VARCHAR(7), NEXT VALUE FOR InvoiceTransactionCodeSequence),
				source.Systempropertyhouseroomid,source.Systempropertyhousetenantid,@FinanceTransactionId,source.Periodid,GETDATE(),GETDATE(),source.TotalAmount,0,0,0,source.TotalAmount,0,'NOT PAID'
				) 
				OUTPUT inserted.Invoiceid INTO @SystemInvoiceiddata;

				-- Fetch the InvoiceId generated
				SET @Invoiceid = (SELECT TOP 1 Invoiceid FROM @SystemInvoiceiddata);

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
		
				INSERT INTO Monthlyrentinvoices(Invoiceno,Propertyhouseroomid,Propertyhouseroomtenantid,Financetransactionid,Datecreated,Duedate,Amount,Discount,Ispaid,Paidamount,Balance,Issent,Paidstatus)
				OUTPUT inserted.Invoiceid INTO @SystemInvoiceiddata
				SELECT 'INV'+''+CONVERT(VARCHAR(7),NEXT VALUE FOR InvoiceTransactionCodeSequence),Systempropertyhouseroomid,Systempropertyhousetenantid,@FinanceTransactionId,GETDATE(),GETDATE(),TotalAmount,0,0,0,TotalAmount,0,'NOT PAID' FROM #MergedData;
				SET @Invoiceid = (SELECT TOP 1 Invoiceid FROM @SystemInvoiceiddata);

				INSERT INTO Monthlyrentinvoiceitems(Invoiceid,Systempropertyhousedepositfeeid,Units,Price,Discount)
				SELECT @Invoiceid, Systempropertyhousedepositfeeid,Units,Amount,Discount
				FROM #MergedData CROSS APPLY OPENJSON(Ticketlines) WITH (Systempropertyhousedepositfeeid BIGINT '$.Systempropertyhousedepositfeeid',Units DECIMAL(18,2) '$.Units', Amount DECIMAL(18,2) '$.Amount',Discount DECIMAL(18,2) '$.Discount',Createdby BIGINT '$.Createdby',Datecreated DATETIME2(6) '$.Datecreated');
		

				UPDATE Systempropertyhouseroomstenant SET Isnewtenant=0 WHERE Systempropertyhousetenantid =(SELECT TOP 1 Systempropertyhousetenantid FROM #MergedData);
            END

            FETCH NEXT FROM Cur INTO @RoomId, @TenantId, @TotalAmount, @RentDueDay, @Ticketlines, @AccountId;
        END

        CLOSE Cur;
        DEALLOCATE Cur;

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