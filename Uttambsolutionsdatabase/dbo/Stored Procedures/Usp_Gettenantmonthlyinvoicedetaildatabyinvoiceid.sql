CREATE PROCEDURE [dbo].[Usp_Gettenantmonthlyinvoicedetaildatabyinvoiceid]
@Invoiceid BIGINT,
@Systemtenantmonthlyinvoicedetaildata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		  SET @Systemtenantmonthlyinvoicedetaildata= 
				  (SELECT(SELECT CASE WHEN (SELECT COUNT(PYM.CustomerPaymentId) FROM Customerpayments PYM WHERE PYM.HouseRoomTenantId=MRI.Propertyhouseroomtenantid AND IsPaymentValidated=0)<1 THEN 0 ELSE 1 END AS Haspendingpayment,MRI.Invoiceid,MRI.Financetransactionid,FT.TransactionCode,MRI.Invoiceno,MRI.Propertyhouseroomid,sizes.Systemhousesizename,room.Systempropertyhousesizename,MRI.Propertyhouseroomtenantid,
					  tenant.Firstname +' ' +tenant.Lastname AS Tenantname,MRI.Datecreated,MRI.Duedate,MRI.Amount,MRI.Discount,MRI.Ispaid,MRI.Paidamount,MRI.Balance,MRI.Issent,MRI.Paidstatus,
					  (SELECT  MRID.Invoiceitemid,MRID.Invoiceid,MRID.Systempropertyhousedepositfeeid,SDF.Housedepositfeename,MRID.Units,MRID.Price,MRID.Discount
					   FROM Monthlyrentinvoiceitems MRID
					   INNER JOIN Systempropertyhousedepositfees SPDF ON MRID.Systempropertyhousedepositfeeid=SPDF.Systempropertyhousedepositfeeid
					   INNER JOIN Systemhousedepositfees SDF ON SPDF.Systempropertyhousedepositfeeid=SDF.Housedepositfeeid
					   WHERE MRI.Invoiceid= MRID.Invoiceid
					   FOR JSON PATH
					  ) AS InvoiceDetails,
					  (SELECT SPBD.Systempropertybankaccountid,SPBD.Propertyhouseid,SPBD.Systembankid,SSB.Systembankname+' - '+CONVERT(VARCHAR(10),SSB.Systembankpaybill) AS Systembanknameandpaybill,SPBD.Systempropertybankaccount,SPBD.Systempropertyhousebankwehave
						FROM Systempropertybankaccounts SPBD INNER JOIN Systemsupportedbanks SSB ON SPBD.Systembankid=SSB.Systembankid
						 WHERE SPBD.Systempropertyhousebankwehave=1 AND SPBD.Propertyhouseid= room.Systempropertyhouseid
						 FOR JSON PATH
						) AS Propertyhousebankingdetail
					  FROM Monthlyrentinvoices MRI
					  INNER JOIN FinanceTransactions FT ON MRI.Financetransactionid= FT.FinanceTransactionId
					  INNER JOIN Systempropertyhouserooms room ON MRI.Propertyhouseroomid= room.Systempropertyhouseroomid
					  INNER JOIN Systempropertyhouses propertyhouse ON room.Systempropertyhouseid= propertyhouse.Propertyhouseid
					  INNER JOIN Systempropertyhousesizes roomsizes ON room.Systempropertyhousesizeid= roomsizes.Systempropertyhousesizeid
					  INNER JOIN Systemhousesizes sizes ON roomsizes.Systemhousesizeid=sizes.Systemhousesizeid
					  INNER JOIN Systemstaffs tenant ON MRI.Propertyhouseroomtenantid= tenant.Userid
					  WHERE MRI.Invoiceid=@Invoiceid
					FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
				 ) AS Data 
				 FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER);
	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systemtenantmonthlyinvoicedetaildata AS Systemtenantmonthlyinvoicedetaildata;

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