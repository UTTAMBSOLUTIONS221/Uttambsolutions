CREATE PROCEDURE [dbo].[Usp_Getsystemstoreitemdatabyid]
@Storeitemid BIGINT,
@Storeitemdata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		    SET @Storeitemdata = (SELECT  SSTI.Storeitemid,SSTI.Storeitemname,(SELECT TOP 1 SII.Storeproductimgurl FROM Systemstoreitemsimages SII WHERE SSTI.Storeitemid=SII.Storeitemid ORDER BY SII.Datecreated) AS Storeproductimgurl,SSTI.Itembrandname,'Sokojiji is the selling point' AS Productdescription,'instock' AS Productavailability, '' AS Productstatus,ISNULL(SSTI.Itemsize,'N/A') AS Itemsize,SSTI.Itembuyingprice,SSTI.Itemsellingprice,SSTI.Itemstatus,SSTI.Isactive,SSTI.Isdeleted,SSTI.Createdby,SSTI.Modifiedby,SSTI.Datecreated,SSTI.Datemodified,
		     (
				SELECT SII.Storeitemid,SII.Storeproductimgurl,SII.Datecreated FROM Systemstoreitemsimages SII
				WHERE SSTI.Storeitemid= SII.Storeitemid
				FOR JSON PATH
				) AS Storeproductimages
				FROM Systemstoreitems SSTI WHERE SSTI.Storeitemid=@Storeitemid
				FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			 );
		   Set @RespMsg ='Ok.'
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