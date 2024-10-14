CREATE PROCEDURE [dbo].[Usp_Getcollectioncenterparcelsdata]
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		SELECT CCP.Parcelid,CCP.Senderid,SENDER.Firstname+' '+ SENDER.Lastname AS Sendername,CCP.Receiverid,RECIEVER.Firstname+' '+ RECIEVER.Lastname AS Recievername,CCP.Collectioncenterid,PICKUP.Collectionname AS Collectionname,
		CCP.Parceltypeid,TYP.Parceltypename,CCP.Parcelweight,CCP.Dimensions,CCP.Parcelstatusid,STAT.Parcelstatusname,CCP.Transitdays,CCP.DeliveryFee,CCP.Trackingnumber,CCP.Pickupdate,CCP.Dropoffdate,CCP.Createddate 
		FROM Parcels CCP
		INNER JOIN Systemstaffs SENDER ON CCP.Senderid=SENDER.Userid
		INNER JOIN Systemstaffs RECIEVER ON CCP.Receiverid=RECIEVER.Userid
		INNER JOIN Parcelcollectioncenters PICKUP ON CCP.Collectioncenterid=PICKUP.Collectioncenterid
		INNER JOIN Parceltypes TYP ON CCP.Parceltypeid=TYP.Parceltypeid
		INNER JOIN Parcelstatus STAT ON CCP.Parcelstatusid=STAT.Parcelstatusid
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