CREATE PROCEDURE [dbo].[Usp_Getsystempropertyroompaymentbypaymentid]
@Paymentid BIGINT,
@Systempropertyroompaymentdata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
			 SET @Systempropertyroompaymentdata= 
				  (SELECT(SELECT CustomerPaymentId,HouseRoomTenantId,HouseRoomTenantId AS Tenantid,HouseRoomId AS Houseroomid,PaymentModeId,FinanceTransactionId AS Financetransactionid,Amount,Actualamount ,TransactionReference,TransactionDate,IsPaymentValidated ,ChequeNo ,ChequeDate,Memo ,DrawerBank,DepositBank,PaidBy,ValidatedBy,SlipReference,DateCreated FROM CustomerPayments WHERE CustomerPaymentId=@Paymentid FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER) AS Data
					FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
				 );
	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systempropertyroompaymentdata AS Systempropertyroompaymentdata;

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