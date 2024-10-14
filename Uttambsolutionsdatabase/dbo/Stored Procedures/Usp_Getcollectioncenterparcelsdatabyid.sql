CREATE PROCEDURE [dbo].[Usp_Getcollectioncenterparcelsdatabyid]
@Parcelid INT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		SELECT CCP.Parcelid,CCP.Senderid,CCP.Receiverid,CCP.Pickupcenterid,CCP.Deliverycenterid,CCP.Parceltypeid,CCP.Parcelweight,CCP.Dimensions,CCP.Parcelstatusid,CCP.Transitdays,CCP.DeliveryFee,CCP.Trackingnumber,CCP.Pickupdate,CCP.Dropoffdate,CCP.Createddate FROM Parcels CCP WHERE CCP.Parcelid= @Parcelid
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