CREATE PROCEDURE [dbo].[Usp_Generatemonthlyrentinvoicedata]
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '',
            @FinanceTransactionId BIGINT,
            @TicketId BIGINT,
            @InvoiceId BIGINT;

    BEGIN TRY
        -- Start a transaction
        BEGIN TRANSACTION;

        -- Declare temporary tables to hold the output from the inserted records
        DECLARE @SystemFinanceTransactionData TABLE (FinanceTransactionId BIGINT);
        DECLARE @SystemTicketData TABLE (TicketId BIGINT);
        DECLARE @SystemInvoiceIdData TABLE (InvoiceId BIGINT);

        -- Create the temporary table to store merged data
        CREATE TABLE #MergedData (
            SystemPropertyHouseRoomId INT,
            SystemPropertyHouseTenantId INT,
            SystemPropertyHouseId INT,
            AccountId INT,
            AccountNumber VARCHAR(50),
            RentDueDay INT,
            TicketLines NVARCHAR(MAX),
            TotalAmount DECIMAL(18, 2)
        );

        -- Insert into #MergedData
        INSERT INTO #MergedData (SystemPropertyHouseRoomId, SystemPropertyHouseTenantId, SystemPropertyHouseId, AccountId, AccountNumber, RentDueDay, TicketLines, TotalAmount)
        SELECT i.SystemPropertyHouseRoomId, i.SystemPropertyHouseTenantId, room.SystemPropertyHouseId, account.AccountId, account.AccountNumber, PropertyHouse.RentDueDay,
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
                               WHEN housedeposit.HouseDepositFeeName IN ('Water Deposit', 'Electricity Deposit', 'Bin Fee', 'Security Fee') THEN deposit.SystemPropertyHouseDepositFeeAmount 
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

        -- Insert multiple records into FinanceTransactions
        INSERT INTO FinanceTransactions(TenantId, TransactionCode, FinanceTransactionTypeId, FinanceTransactionSubTypeId, ParentId, SaleDescription, IsOnlineSale, CreatedBy, ActualDate, DateCreated)
        OUTPUT inserted.FinanceTransactionId INTO @SystemFinanceTransactionData
        SELECT SystemPropertyHouseTenantId, 'TXN' + CONVERT(VARCHAR(70), NEXT VALUE FOR TransactionCodeSequence),
               (SELECT TOP 1 FinanceTransactionTypeId FROM FinanceTransactionTypes WHERE FinanceTransactionType='Monthly Rent'),(SELECT TOP 1 FinanceTransactionSubTypeId FROM FinanceTransactionSubTypes WHERE FinanceTransactionSubType='Bill Generattion'),
               0, 'New Tenant House Rent, Deposits and Other Deposit', 1, 0, GETDATE(), GETDATE()
        FROM #MergedData;

        -- Insert multiple records into GLTransactions
        INSERT INTO GLTransactions(FinanceTransactionId, ChartOfAccountId, PeriodId, Amount, GlActualDate, DateCreated)
        SELECT f.FinanceTransactionId, COALESCE(c.ChartOfAccountId, (SELECT ChartOfAccountId FROM ChartOfAccounts WHERE ChartOfAccountName = 'Accounts Payable')), 1, m.TotalAmount, GETDATE(), GETDATE()
        FROM @SystemFinanceTransactionData f
        CROSS JOIN #MergedData m
        LEFT JOIN ChartOfAccounts c ON c.ChartOfAccountName = m.AccountNumber;

        -- Insert multiple records into SystemTickets
        INSERT INTO SystemTickets(FinanceTransactionId, HouseRoomId, AccountId, CreatedBy, ActualDate, DateCreated)
        OUTPUT inserted.TicketId INTO @SystemTicketData
        SELECT f.FinanceTransactionId, m.SystemPropertyHouseRoomId, m.AccountId, 0, GETDATE(), GETDATE()
        FROM @SystemFinanceTransactionData f
        CROSS JOIN #MergedData m;

        -- Insert multiple records into TicketLines
        INSERT INTO TicketLines (TicketId, SystemPropertyHouseDepositFeeId, Units, Price, Discount, CreatedBy, DateCreated)
        SELECT t.TicketId, j.SystemPropertyHouseDepositFeeId, j.Units, j.Amount, j.Discount, j.CreatedBy, j.DateCreated
        FROM #MergedData m
        CROSS APPLY OPENJSON(m.TicketLines)
        WITH (
            SystemPropertyHouseDepositFeeId BIGINT '$.SystemPropertyHouseDepositFeeId',
            Units DECIMAL(18, 2) '$.Units',
            Amount DECIMAL(18, 2) '$.Amount',
            Discount DECIMAL(18, 2) '$.Discount',
            CreatedBy BIGINT '$.CreatedBy',
            DateCreated DATETIME2(6) '$.DateCreated'
        ) j
        CROSS JOIN @SystemTicketData t;

        -- Insert multiple records into MonthlyRentInvoices
        INSERT INTO MonthlyRentInvoices (InvoiceNo, PropertyHouseRoomId, Propertyhouseroomtenantid, FinanceTransactionId, DateCreated, DueDate, Amount, Discount, IsPaid, PaidAmount, Balance, IsSent, PaidStatus)
        OUTPUT inserted.InvoiceId INTO @SystemInvoiceIdData
        SELECT 'INV' + CONVERT(VARCHAR(7), NEXT VALUE FOR InvoiceTransactionCodeSequence), m.SystemPropertyHouseRoomId, m.SystemPropertyHouseTenantId, f.FinanceTransactionId,
               GETDATE(), GETDATE(), m.TotalAmount, 0, 0, 0, m.TotalAmount, 0, 'NOT PAID'
        FROM #MergedData m
        CROSS JOIN @SystemFinanceTransactionData f;

        -- Insert multiple records into MonthlyRentInvoiceItems
        INSERT INTO MonthlyRentInvoiceItems (InvoiceId, SystemPropertyHouseDepositFeeId, Units, Price, Discount)
        SELECT inv.InvoiceId, j.SystemPropertyHouseDepositFeeId, j.Units, j.Amount, j.Discount
        FROM #MergedData m
        CROSS APPLY OPENJSON(m.TicketLines)
        WITH (
            SystemPropertyHouseDepositFeeId BIGINT '$.SystemPropertyHouseDepositFeeId',
            Units DECIMAL(18, 2) '$.Units',
            Amount DECIMAL(18, 2) '$.Amount',
            Discount DECIMAL(18, 2) '$.Discount',
            CreatedBy BIGINT '$.CreatedBy',
            DateCreated DATETIME2(6) '$.DateCreated'
        ) j
        CROSS JOIN @SystemInvoiceIdData inv;

        -- Update SystemPropertyHouseRoomTenant for the processed tenants
        UPDATE Systempropertyhouseroomstenant
        SET Lastinvoiceddate = GETDATE()
        WHERE Systempropertyhousetenantid IN (SELECT SystemPropertyHouseTenantId FROM #MergedData);

        -- Commit the transaction if all steps succeeded
        COMMIT TRANSACTION;

        -- Return success response
        SET @RespStat = 1;
        SET @RespMsg = 'Monthly rent invoice data generated successfully';
    END TRY
    BEGIN CATCH
        -- Rollback transaction on error
        IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

        -- Set error response
        SET @RespStat = 0;
        SET @RespMsg = ERROR_MESSAGE();
    END CATCH;

    -- Return the response status and message
    SELECT @RespStat AS ResponseStatus, @RespMsg AS ResponseMessage;
END;