CREATE PROCEDURE [dbo].[Usp_Getsystemservicesitemsdatabyid]
@Serviceid INT,
@Systemservicesitemsdata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		--validate	

		BEGIN TRANSACTION;
		  SET @Systemservicesitemsdata = 
		    (SELECT(SELECT SVI.Serviceitemid,SVI.Serviceid,SVI.Serviceitemname,SVI.Serviceitemimageurl 
			FROM Systemservicesitems SVI WHERE SVI.Serviceid=@Serviceid
			FOR JSON PATH
			) AS Data
			FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			);
	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;
           SELECT  @RespStat as RespStatus, @RespMsg as RespMessage;  
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