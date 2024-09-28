CREATE PROCEDURE [dbo].[Usp_Getsystemservicesdatabyid]
@Serviceid BIGINT,
@Systemservicedata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		    SET @Systemservicedata = (SELECT SVC.Serviceid,SVC.Servicename,SVC.Subscriptionfee,SVC.Isvisible,
		     (
				SELECT SVCI.Serviceitemid,SVCI.Serviceid,SVCI.Serviceitemname,SVCI.Serviceitemimageurl FROM Systemservicesitems SVCI
				WHERE SVC.Serviceid= SVCI.Serviceid
				FOR JSON PATH
				) AS Serviceitems
				FROM Systemservices SVC WHERE SVC.Serviceid=@Serviceid
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