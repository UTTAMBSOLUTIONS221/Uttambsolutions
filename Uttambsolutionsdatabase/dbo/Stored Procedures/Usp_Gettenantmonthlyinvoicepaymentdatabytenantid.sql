CREATE PROCEDURE [dbo].[Usp_Gettenantmonthlyinvoicepaymentdatabytenantid]
@Tenantid BIGINT,
@Systemtenantmonthlyinvoicepaymentdata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		  SET @Systemtenantmonthlyinvoicepaymentdata= 
				  (SELECT (SELECT CTPD.CustomerPaymentId,CTPD.HouseRoomTenantId,STF.Firstname+' '+ STF.Lastname AS Housetenantname,CTPD.HouseRoomId,OWN.Firstname+' '+ OWN.Lastname AS Houseownername,
					SPH.Propertyhousename,SPHR.Systempropertyhousesizename,SHS.Systemhousesizename,CTPD.PaymentModeId,PYM.Paymentmode,CTPD.FinanceTransactionId,FTS.TransactionCode,CTPD.Amount,
					CTPD.TransactionReference,CTPD.TransactionDate,CTPD.IsPaymentValidated,CTPD.ChequeNo,CTPD.ChequeDate,CTPD.Memo,CTPD.DrawerBank,CTPD.DepositBank,CTPD.PaidBy,CTPD.SlipReference,CTPD.DateCreated
					FROM CustomerPayments CTPD
					INNER JOIN Paymentmodes PYM ON CTPD.PaymentModeId=PYM.PaymentmodeId
					INNER JOIN Systemstaffs STF ON CTPD.HouseRoomTenantId=STF.Userid
					INNER JOIN Systempropertyhouserooms SPHR ON CTPD.HouseRoomId=SPHR.Systempropertyhouseroomid
					INNER JOIN Systempropertyhousesizes SPS ON SPHR.Systempropertyhousesizeid=SPS.Systempropertyhousesizeid
					INNER JOIN Systemhousesizes SHS ON SPS.Systemhousesizeid=SHS.Systemhousesizeid
					INNER JOIN Systempropertyhouses SPH ON SPHR.Systempropertyhouseid=SPH.Propertyhouseid
					INNER JOIN Systemstaffs OWN ON SPH.Propertyhouseowner=OWN.Userid
					INNER JOIN FinanceTransactions FTS ON CTPD.FinanceTransactionId=FTS.FinanceTransactionId
					WHERE CTPD.HouseRoomTenantId=@Tenantid
					FOR JSON PATH
				 ) AS Data
				 FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
				 );

	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systemtenantmonthlyinvoicepaymentdata AS Systemtenantmonthlyinvoicepaymentdata;

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
