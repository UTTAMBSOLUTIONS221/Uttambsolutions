CREATE TABLE [dbo].[Systempropertyhouseroomstenant] (
    [Systempropertyhousetenantentryid] BIGINT        IDENTITY (1, 1) NOT NULL,
    [Systempropertyhousetenantid]      BIGINT        NOT NULL,
    [Systempropertyhouseroomid]        INT           NOT NULL,
    [Isoccupant]                       BIT           DEFAULT ((0)) NOT NULL,
    [Occupationalstatus]               INT           NOT NULL,
    [Isnewtenant]                      BIT           DEFAULT ((0)) NOT NULL,
    [Ratingvalue]                      DECIMAL(10, 2) DEFAULT ((0)) NOT NULL,
    [Ownercomment]                     VARCHAR (200) DEFAULT ('Not Set') NOT NULL,
    [Createdby]                        INT           NOT NULL,
    [Modifiedby]                       INT           NOT NULL,
    [Vacateddate]                      DATETIME      NOT NULL,
    [Datecreated]                      DATETIME      NOT NULL,
    [Datemodified]                     DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Systempropertyhousetenantentryid] ASC)
);


GO

CREATE TRIGGER [dbo].[trg_AfterInsert_Generatesystemtenantinvoicebillfornewtenant]
ON [dbo].[Systempropertyhouseroomstenant]
AFTER INSERT
AS
BEGIN
  DECLARE @RespStat INT = 0,
          @RespMsg VARCHAR(150) = '',
		  @FinanceTransactionId BIGINT,
		  @Ticketid BIGINT,
		  @Invoiceid BIGINT;
    BEGIN TRY
        -- Validate

        BEGIN TRANSACTION;
		
        DECLARE @SystemfinanceTransactiondata TABLE (FinanceTransactionId BIGINT);
        DECLARE @Systemticketdata TABLE (Ticketid BIGINT);
		DECLARE  @SystemInvoiceiddata TABLE (Invoiceid BIGINT);
         CREATE TABLE #MergedData (Systempropertyhouseroomid INT,Systempropertyhousetenantid INT,Systempropertyhouseid INT,Accountid INT,Accountnumber VARCHAR(50),Rentdueday INT,Ticketlines NVARCHAR(MAX), TotalAmount DECIMAL(18, 2));
		 INSERT INTO #MergedData (Systempropertyhouseroomid,Systempropertyhousetenantid,Systempropertyhouseid,Accountid,Accountnumber,Rentdueday,Ticketlines,TotalAmount)
		 SELECT i.Systempropertyhouseroomid,i.Systempropertyhousetenantid,room.Systempropertyhouseid,account.Accountid,account.Accountnumber,Propertyhouse.Rentdueday,
			(
				-- Fetch non-recurring ticket lines as JSON, including house rent
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
				INNER JOIN Systempropertyhousesizes rental ON deposit.Propertyhouseid = rental.Propertyhouseid
				INNER JOIN  Systempropertyhouserooms room ON rental.Systempropertyhousesizeid = room.Systempropertyhousesizeid
				INNER JOIN Systempropertyhouses  Propertyhouse ON room.Systempropertyhouseid=Propertyhouse.Propertyhouseid
				WHERE room.Systempropertyhouseroomid = i.Systempropertyhouseroomid AND (housedeposit.Isrecurring = 0 OR housedeposit.Housedepositfeename = 'House Rent') -- Include non-recurring items and house rent
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
					WHERE room.Systempropertyhouseroomid = i.Systempropertyhouseroomid  AND (housedeposit.Isrecurring = 0 OR housedeposit.Housedepositfeename = 'House Rent') -- Include non-recurring items and house rent
				) AS NonRecurringAmountsSubquery
			) AS NonRecurringTotalAmount
		FROM inserted i
		INNER JOIN   Systempropertyhouserooms room ON i.Systempropertyhouseroomid = room.Systempropertyhouseroomid
		INNER JOIN Systempropertyhousesizes rental ON room.Systempropertyhousesizeid = rental.Systempropertyhousesizeid
		INNER JOIN Systempropertyhouses  Propertyhouse ON room.Systempropertyhouseid=Propertyhouse.Propertyhouseid
		INNER JOIN Systemstaffsaccount account ON i.Systempropertyhousetenantid = account.Userid;
		
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

		DROP TABLE #MergedData;

        SET @RespMsg = 'Success';
        SET @RespStat = 0; 
        COMMIT TRANSACTION;

        SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT '';
        PRINT 'Error ' + ERROR_MESSAGE();
        SELECT 2 AS RespStatus, '0 - Error(s) Occurred: ' + ERROR_MESSAGE() AS RespMessage;
    END CATCH

    SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;
    RETURN;
END

