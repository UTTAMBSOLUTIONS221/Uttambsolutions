CREATE PROCEDURE [dbo].[Usp_Verifystaffaccountdatabyid]
    @Staffid BIGINT
AS
BEGIN
   BEGIN
	DECLARE  
			@RespStat int = 0,
			@RespMsg varchar(150) = '';

			BEGIN
	
		BEGIN TRY	
		--Validate

		BEGIN TRANSACTION;
		BEGIN
         UPDATE Systemstaffs SET Loginstatus =0 WHERE Userid =@Staffid
		END
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