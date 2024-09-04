CREATE PROCEDURE [dbo].[Usp_Registerpropertyhouseroomrentpaymentrequestdata]
 @JsonObjectdata NVARCHAR(MAX)
AS
BEGIN
   BEGIN
	DECLARE 
	        @FinanceTransactionId BIGINT,
	        @Accountnumber BIGINT,
			@CustomerPaymentId BIGINT,
			@RespStat int = 0,
			@RespMsg varchar(150) = ''
			BEGIN
		BEGIN TRY	
		--Validate
		IF EXISTS(SELECT CustomerPaymentId FROM CustomerPayments WHERE Transactionreference = JSON_VALUE(@JsonObjectdata, '$.Transactionreference'))
		BEGIN
		 	Select 1 as RespStatus, 'Similar transaction code exists' as RespMessage;
		    return;
		END
		--IF NOT EXISTS(SELECT PeriodId FROM SystemPeriods WHERE Lastdateinperiod =(SELECT DATEADD(SECOND, -1, DATEADD(DAY, 1, CAST(EOMONTH((SELECT dbo.getlocaldate((SELECT TOP 1 c.Utcname FROM SystemStaffs a  INNER JOIN Tenantaccounts b ON a.Tenantid=b.Tenantid INNER JOIN SystemCountry c ON b.Countryid=c.CountryId WHERE a.Tenantid=JSON_VALUE(@JsonObjectdata, '$.TenantId')))), 0) AS datetime)))))
		--BEGIN
		-- INSERT INTO SystemPeriods 
		-- SELECT  DATEADD(SECOND, -1, DATEADD(DAY, 1, CAST(EOMONTH((SELECT dbo.getlocaldate((SELECT TOP 1 c.Utcname FROM SystemStaffs a  INNER JOIN Tenantaccounts b ON a.Tenantid=b.Tenantid INNER JOIN SystemCountry c ON b.Countryid=c.CountryId WHERE a.Tenantid=JSON_VALUE(@JsonObjectdata, '$.TenantId')))), 0) AS datetime)));
		--END
		BEGIN TRANSACTION;
		DECLARE @SystemfinanceTransactiondata TABLE (FinanceTransactionId BIGINT);
		INSERT INTO FinanceTransactions(Tenantid,TransactionCode,FinanceTransactionTypeId,FinanceTransactionSubTypeId,ParentId,Saledescription,IsOnlineSale,Createdby,ActualDate,DateCreated)
		OUTPUT inserted.FinanceTransactionId INTO @SystemfinanceTransactiondata
		SELECT JSON_VALUE(@JsonObjectdata, '$.Tenantid'),'PAY'+''+CONVERT(VARCHAR(70),NEXT VALUE FOR PaymentTransactionCodeSequence),(SELECT TOP 1 FinanceTransactionTypeId FROM FinanceTransactionTypes WHERE FinanceTransactionType='Monthly Rent'),(SELECT TOP 1 FinanceTransactionSubTypeId FROM FinanceTransactionSubTypes WHERE FinanceTransactionSubType='Bill Generattion'),0, 'New Tenant House Rent, House Deposits and Other Deposit', 1, 0, TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Datecreated') AS datetime2(6)),TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Datecreated') AS datetime2(6))
		SET @FinanceTransactionId = (SELECT TOP 1 FinanceTransactionId FROM @SystemfinanceTransactiondata);
	
		--inserting to Gl Transactions
		INSERT INTO GLTransactions(FinanceTransactionId,ChartofAccountId,PeriodId,Amount,GlActualDate,DateCreated)
		SELECT @FinanceTransactionId,(SELECT ChartofAccountId FROM ChartofAccounts WHERE ChartofAccountname = (SELECT TOP 1 CONVERT(VARCHAR(10),sacc.Accountnumber) FROM Systemstaffsaccount sacc WHERE sacc.Userid=JSON_VALUE(@JsonObjectdata, '$.Tenantid'))),1,-1 * CONVERT(decimal(10,4),(JSON_VALUE(@JsonObjectdata, '$.Amount'))),TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Datecreated') AS datetime2(6)),TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Datecreated') AS datetime2(6));

		INSERT INTO GLTransactions(FinanceTransactionId,ChartofAccountId,PeriodId,Amount,GlActualDate,DateCreated)
		SELECT @FinanceTransactionId,(SELECT ChartofAccountId FROM ChartofAccounts WHERE ChartofAccountname='Accounts Receivable'),1,-1 * CONVERT(decimal(10,4),(JSON_VALUE(@JsonObjectdata, '$.Amount'))),TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Datecreated') AS datetime2(6)),TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Datecreated') AS datetime2(6));

		INSERT INTO CustomerPayments(HouseRoomTenantId,HouseRoomId,PaymentModeId,FinanceTransactionId,Amount,Actualamount,TransactionReference,TransactionDate,IsPaymentValidated,ChequeNo,ChequeDate,Memo,DrawerBank,DepositBank,PaidBy,ValidatedBy,SlipReference,DateCreated)
		VALUES(JSON_VALUE(@JsonObjectdata, '$.Tenantid'),JSON_VALUE(@JsonObjectdata, '$.Houseromid'),JSON_VALUE(@JsonObjectdata, '$.Paymentmodeid'),@FinanceTransactionId,JSON_VALUE(@JsonObjectdata, '$.Amount'),0,JSON_VALUE(@JsonObjectdata, '$.Transactionreference'),CAST(JSON_VALUE(@JsonObjectdata, '$.Transactiondate') AS datetime2),JSON_VALUE(@JsonObjectdata, '$.Ispaymentvalidated'),
		JSON_VALUE(@JsonObjectdata, '$.Chequeno'),CAST(JSON_VALUE(@JsonObjectdata, '$.Chequedate') AS datetime2),JSON_VALUE(@JsonObjectdata, '$.Memo'),JSON_VALUE(@JsonObjectdata, '$.Drawerbank'),JSON_VALUE(@JsonObjectdata, '$.Depositbank'),JSON_VALUE(@JsonObjectdata, '$.Paidby'),JSON_VALUE(@JsonObjectdata, '$.Paidby'),JSON_VALUE(@JsonObjectdata, '$.Slipreference'),CAST(JSON_VALUE(@JsonObjectdata, '$.Datecreated') AS datetime2))
		SET @CustomerPaymentId = SCOPE_IDENTITY();

		Set @RespMsg ='Success'
		Set @RespStat =0; 
		COMMIT TRANSACTION;
		Select @RespStat as RespStatus, @RespMsg as RespMessage;

		END TRY
		BEGIN CATCH
		ROLLBACK TRANSACTION
		PRINT ''
		PRINT 'Error ' + error_message();
		Select 2 as RespStatus, '0 - Error(s) Occurred' + error_message() as RespMessage
		END CATCH
		Select @RespStat as RespStatus, @RespMsg as RespMessage;
		RETURN; 
		END;
	END
END