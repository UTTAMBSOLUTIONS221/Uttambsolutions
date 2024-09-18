CREATE PROCEDURE [dbo].[Usp_Generatemonthlyrentinvoicedata]
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '',
            @FinanceTransactionId BIGINT,
            @TicketId BIGINT,
            @InvoiceId BIGINT,
            @SystemPropertyHouseRoomId INT,
            @SystemPropertyHouseTenantId INT,
            @SystemPropertyHouseId INT,
            @AccountId INT,
			@Periodid INT,
			@Invoicestatus VARCHAR(20),
            @AccountNumber VARCHAR(50),
            @RentDueDay INT,
            @TicketLines NVARCHAR(MAX),
            @TotalAmount DECIMAL(18, 2);

    BEGIN TRY
        -- Start a transaction
        BEGIN TRANSACTION;
		DECLARE @SystemFinanceTransactionData TABLE (FinanceTransactionId BIGINT);
        DECLARE @SystemTicketData TABLE (TicketId BIGINT);
        DECLARE @SystemInvoiceIdData TABLE (InvoiceId BIGINT);

        -- Create the temporary table to store merged data
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
        -- Insert into #MergedData
        INSERT INTO #MergedData (SystemPropertyHouseRoomId, SystemPropertyHouseTenantId, SystemPropertyHouseId, AccountId, AccountNumber, RentDueDay,Periodid,Invoicestatus, TicketLines, TotalAmount)
        SELECT i.SystemPropertyHouseRoomId, i.SystemPropertyHouseTenantId, room.SystemPropertyHouseId, account.AccountId, account.AccountNumber, PropertyHouse.RentDueDay,(SELECT x.PeriodId FROM Systemperiods x WHERE x.Lastdateinperiod=(SELECT EOMONTH(GETDATE(), 0))) AS Periodid,'Newinvoice' AS Invoicestatus,
               (
                   -- Fetch non-recurring ticket lines as JSON, including house rent
                   SELECT deposit.SystemPropertyHouseDepositFeeId, housedeposit.HouseDepositFeeName AS Name,
                          CASE 
                              WHEN housedeposit.HouseDepositFeeName IN ('Water Deposit', 'Electricity Deposit', 'House Rent', 'Bin Fee', 'Security Fee') THEN 1 
                              WHEN housedeposit.HouseDepositFeeName = 'Water Bill' THEN 0 
                              ELSE 1 
                          END AS Units,
                          CASE 
                              WHEN housedeposit.HouseDepositFeeName IN ('Water Deposit', 'Electricity Deposit', 'Bin Fee', 'Security Fee') THEN deposit.SystemPropertyHouseDepositFeeAmount 
                              WHEN housedeposit.HouseDepositFeeName = 'House Rent' THEN room.SystemPropertyHouseSizeRent 
                              WHEN housedeposit.HouseDepositFeeName = 'Water Bill' THEN 0
                              ELSE 0 
                          END AS Amount,
                          0 AS Discount,
                          0 AS CreatedBy,
                          GETDATE() AS DateCreated,
                          housedeposit.IsRecurring
                   FROM SystemPropertyHouseDepositFees deposit 
                   INNER JOIN SystemHouseDepositFees housedeposit ON deposit.HouseDepositFeeId = housedeposit.HouseDepositFeeId 
                   INNER JOIN SystemPropertyHouseSizes rental ON deposit.PropertyHouseId = rental.PropertyHouseId
                   INNER JOIN SystemPropertyHouseRooms room ON rental.SystemPropertyHouseSizeId = room.SystemPropertyHouseSizeId
                   INNER JOIN SystemPropertyHouses PropertyHouse ON room.SystemPropertyHouseId = PropertyHouse.PropertyHouseId
                   WHERE room.SystemPropertyHouseRoomId = i.SystemPropertyHouseRoomId AND PropertyHouse.HasHouseWaterMeter = 0 AND housedeposit.IsRecurring = 1
                   FOR JSON PATH
               ) AS TicketLines,
               (
                   -- Calculate the total amount for non-recurring items, including house rent
                   SELECT SUM(Amount) AS NonRecurringTotal
                   FROM (
                       SELECT 
                           CASE 
                               WHEN housedeposit.HouseDepositFeeName IN ('Bin Fee', 'Security Fee') THEN deposit.SystemPropertyHouseDepositFeeAmount 
                               WHEN housedeposit.HouseDepositFeeName = 'House Rent' THEN room.SystemPropertyHouseSizeRent 
                               ELSE 0 
                           END AS Amount
                       FROM SystemPropertyHouseDepositFees deposit 
                       INNER JOIN SystemHouseDepositFees housedeposit ON deposit.HouseDepositFeeId = housedeposit.HouseDepositFeeId 
                       INNER JOIN SystemPropertyHouseSizes rental ON deposit.PropertyHouseId = rental.PropertyHouseId
                       INNER JOIN SystemPropertyHouseRooms room ON rental.SystemPropertyHouseSizeId = room.SystemPropertyHouseSizeId 
                       WHERE room.SystemPropertyHouseRoomId = i.SystemPropertyHouseRoomId AND PropertyHouse.HasHouseWaterMeter = 0 AND housedeposit.IsRecurring = 1
                   ) AS NonRecurringAmountsSubquery
               ) AS TotalAmount
        FROM Systempropertyhouseroomstenant i
        INNER JOIN SystemPropertyHouseRooms room ON i.SystemPropertyHouseRoomId = room.SystemPropertyHouseRoomId
        INNER JOIN SystemPropertyHouseSizes rental ON room.SystemPropertyHouseSizeId = rental.SystemPropertyHouseSizeId
        INNER JOIN SystemPropertyHouses PropertyHouse ON room.SystemPropertyHouseId = PropertyHouse.PropertyHouseId
        INNER JOIN Systemstaffsaccount account ON i.SystemPropertyHouseTenantId = account.UserId
        WHERE PropertyHouse.HasHouseWaterMeter = 0;

        -- Cursor to loop over #MergedData
        DECLARE DataCursor CURSOR FOR 
        SELECT SystemPropertyHouseRoomId, SystemPropertyHouseTenantId, SystemPropertyHouseId, AccountId, AccountNumber, RentDueDay,Periodid,Invoicestatus, TicketLines, TotalAmount
        FROM #MergedData;

        OPEN DataCursor;

        FETCH NEXT FROM DataCursor INTO @SystemPropertyHouseRoomId, @SystemPropertyHouseTenantId, @SystemPropertyHouseId, @AccountId, @AccountNumber, @RentDueDay,@Periodid,@Invoicestatus, @TicketLines, @TotalAmount;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Insert into FinanceTransactions with unique TransactionCode for each row
            INSERT INTO FinanceTransactions(TenantId, TransactionCode, FinanceTransactionTypeId, FinanceTransactionSubTypeId, ParentId, SaleDescription, IsOnlineSale, CreatedBy, ActualDate, DateCreated)
            VALUES (@SystemPropertyHouseTenantId, 'TXN' + CONVERT(VARCHAR(70), NEXT VALUE FOR TransactionCodeSequence),
                    (SELECT TOP 1 FinanceTransactionTypeId FROM FinanceTransactionTypes WHERE FinanceTransactionType='Monthly Rent'),
                    (SELECT TOP 1 FinanceTransactionSubTypeId FROM FinanceTransactionSubTypes WHERE FinanceTransactionSubType='Bill Generattion'),
                    0, 'Subsequent Tenant House Rent,', 1, 0, GETDATE(), GETDATE());
					SET @FinanceTransactionId = SCOPE_IDENTITY();

            -- Insert into GLTransactions
            INSERT INTO GLTransactions(FinanceTransactionId, ChartOfAccountId, PeriodId, Amount, GlActualDate, DateCreated)
            SELECT @FinanceTransactionId, COALESCE(c.ChartOfAccountId, (SELECT ChartOfAccountId FROM ChartOfAccounts WHERE ChartOfAccountName = 'Accounts Payable')), 1, @TotalAmount, GETDATE(), GETDATE()
            FROM ChartOfAccounts c
            WHERE c.ChartOfAccountName = @AccountNumber;

            -- Insert into SystemTickets
            INSERT INTO SystemTickets(FinanceTransactionId, HouseRoomId, AccountId, CreatedBy, ActualDate, DateCreated)
            VALUES (@FinanceTransactionId, @SystemPropertyHouseRoomId, @AccountId, 0, GETDATE(), GETDATE());
			SET @TicketId = SCOPE_IDENTITY();

            -- Insert into TicketLines
            INSERT INTO TicketLines (TicketId, SystemPropertyHouseDepositFeeId, Units, Price, Discount, CreatedBy, DateCreated)
            SELECT @TicketId, j.SystemPropertyHouseDepositFeeId, j.Units, j.Amount, j.Discount, j.CreatedBy, j.DateCreated
            FROM OPENJSON(@TicketLines)
            WITH (
                SystemPropertyHouseDepositFeeId BIGINT '$.SystemPropertyHouseDepositFeeId',
                Units DECIMAL(18, 2) '$.Units',
                Amount DECIMAL(18, 2) '$.Amount',
                Discount DECIMAL(18, 2) '$.Discount',
                CreatedBy BIGINT '$.CreatedBy',
                DateCreated DATETIME2(6) '$.DateCreated'
            ) j;

            -- Insert into MonthlyRentInvoices
           INSERT INTO Monthlyrentinvoices(Invoiceno,Propertyhouseroomid,Propertyhouseroomtenantid,Financetransactionid,Periodid,Invoicestatus,Datecreated,Duedate,Amount,Discount,Ispaid,Paidamount,Balance,Issent,Paidstatus)
		   VALUES ('INV' + CONVERT(VARCHAR(7), NEXT VALUE FOR InvoiceTransactionCodeSequence), @SystemPropertyHouseRoomId, @SystemPropertyHouseTenantId, @FinanceTransactionId,(SELECT x.PeriodId FROM Systemperiods x WHERE x.Lastdateinperiod=(SELECT EOMONTH(GETDATE(), 0))),@Invoicestatus,GETDATE(), GETDATE(), @TotalAmount, 0, 0, 0, @TotalAmount, 0, 'NOT PAID');

			SET @InvoiceId = SCOPE_IDENTITY();
            -- Insert into MonthlyRentInvoiceItems
            INSERT INTO MonthlyRentInvoiceItems (InvoiceId, SystemPropertyHouseDepositFeeId, Units, Price, Discount)
            SELECT @InvoiceId, j.SystemPropertyHouseDepositFeeId, j.Units, j.Amount, j.Discount
            FROM OPENJSON(@TicketLines)
            WITH (SystemPropertyHouseDepositFeeId BIGINT '$.SystemPropertyHouseDepositFeeId', Units DECIMAL(18, 2) '$.Units',Amount DECIMAL(18, 2) '$.Amount',Discount DECIMAL(18, 2) '$.Discount') j;
			 
            -- Fetch the next row from the cursor
            FETCH NEXT FROM DataCursor INTO @SystemPropertyHouseRoomId, @SystemPropertyHouseTenantId, @SystemPropertyHouseId, @AccountId, @AccountNumber, @RentDueDay, @TicketLines, @TotalAmount;
        END;

        -- Clean up the cursor
        CLOSE DataCursor;
        DEALLOCATE DataCursor;

        -- Clean up the temporary table
        DROP TABLE #MergedData;

        -- Commit the transaction
        COMMIT TRANSACTION;

        -- Set response status and message
        SET @RespStat = 1;
        SET @RespMsg = 'Monthly rent invoice data generated successfully.';

    END TRY
    BEGIN CATCH
        -- Rollback the transaction in case of an error
        ROLLBACK TRANSACTION;
        -- Handle the error
        SET @RespStat = 0;
        SET @RespMsg = ERROR_MESSAGE();
    END CATCH;

    -- Return the response
    SELECT @RespStat AS Status, @RespMsg AS Message;
END;