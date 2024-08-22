CREATE PROCEDURE [dbo].[Usp_Gettenantmonthlyinvoicedatabyownerid]
@Ownerid BIGINT,
@Systemtenantmonthlyinvoicedata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		  SET @Systemtenantmonthlyinvoicedata= 
				  (SELECT (SELECT MRI.Invoiceid,MRI.Financetransactionid,FT.TransactionCode,MRI.Invoiceno,MRI.Propertyhouseroomid,sizes.Systemhousesizename,room.Systempropertyhousesizename,MRI.Propertyhouseroomtenantid,
					  tenant.Firstname +' ' +tenant.Lastname AS Tenantname,MRI.Datecreated,MRI.Duedate,MRI.Amount,MRI.Discount,MRI.Ispaid,MRI.Paidamount,MRI.Balance,MRI.Issent,MRI.Paidstatus
					  FROM Monthlyrentinvoices MRI
					  INNER JOIN FinanceTransactions FT ON MRI.Financetransactionid= FT.FinanceTransactionId
					  INNER JOIN Systempropertyhouserooms room ON MRI.Propertyhouseroomid= room.Systempropertyhouseroomid
					  INNER JOIN Systempropertyhouses propertyhouse ON room.Systempropertyhouseid= propertyhouse.Propertyhouseid
					  INNER JOIN Systempropertyhousesizes roomsizes ON room.Systempropertyhousesizeid= roomsizes.Systempropertyhousesizeid
					  INNER JOIN Systemhousesizes sizes ON roomsizes.Systemhousesizeid=sizes.Systemhousesizeid
					  INNER JOIN Systemstaffs tenant ON MRI.Propertyhouseroomtenantid= tenant.Userid
					  WHERE propertyhouse.Propertyhouseowner=@Ownerid
					FOR JSON PATH
				 ) AS Data
				 FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
				 );

	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systemtenantmonthlyinvoicedata AS Systemtenantmonthlyinvoicedata;

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
