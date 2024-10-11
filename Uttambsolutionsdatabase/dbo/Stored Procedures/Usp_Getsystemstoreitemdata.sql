CREATE PROCEDURE [dbo].[Usp_Getsystemstoreitemdata]
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Success';
			BEGIN
	
		BEGIN TRY
		--validate	
		
		BEGIN TRANSACTION;
		SELECT  Storeitemid,Storeitemname,Itembrandname,ISNULL(Itemsize,'N/A') AS Itemsize,Itembuyingprice,Itemsellingprice,Itemstatus,Isactive,Isdeleted,Createdby,Modifiedby,Datecreated,Datemodified FROM Systemstoreitems
	    Set @RespMsg ='Success'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage

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