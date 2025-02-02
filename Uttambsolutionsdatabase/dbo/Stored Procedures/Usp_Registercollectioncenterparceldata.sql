﻿CREATE PROCEDURE [dbo].[Usp_Registercollectioncenterparceldata]
@JsonObjectdata VARCHAR(MAX)
AS
BEGIN
   BEGIN
	DECLARE @Parcelid INT,
	        @Collectioncenterid INT,
	        @RespStat int = 0,
			@RespMsg varchar(150) = '';
		  
	BEGIN
		BEGIN TRY	
		--Validate
		IF NOT EXISTS(SELECT Managerid FROM Parcelcollectioncenters WHERE Managerid = JSON_VALUE(@JsonObjectData, '$.Createdby'))
		BEGIN
			SELECT 1 AS RespStatus, 'You dont have a Collection center!' AS RespMessage;
			RETURN;
		END
		ELSE
		BEGIN
		 SELECT @Collectioncenterid=Collectioncenterid FROM Parcelcollectioncenters WHERE Managerid = JSON_VALUE(@JsonObjectData, '$.Createdby')
		END

		BEGIN TRANSACTION;
		    DECLARE @Parcels TABLE (Parcelid INT);
			MERGE INTO Parcels AS target
			USING (
			SELECT Parcelid,Senderid,Receiverid,ISNULL(Collectioncenterid,@Collectioncenterid) AS Collectioncenterid,Parceltypeid,Parcelweight,Dimensions,Parcelstatusid,Transitdays,Deliveryfee,Trackingnumber,Pickupdate,Dropoffdate,Createddate
			FROM OPENJSON(@JsonObjectdata)
			WITH (Parcelid INT '$.Parcelid',Senderid INT '$.Senderid',Receiverid BIGINT '$.Receiverid',Collectioncenterid INT '$.Collectioncenterid',Parceltypeid INT '$.Parceltypeid',Parcelweight DECIMAL(10,2) '$.Parcelweight',Dimensions VARCHAR(50) '$.Dimensions',Parcelstatusid INT '$.Parcelstatusid',Transitdays INT '$.Transitdays',Deliveryfee DECIMAL(10,2) '$.Deliveryfee',Trackingnumber VARCHAR(50) '$.Trackingnumber',Pickupdate DATETIME2 '$.Pickupdate',Dropoffdate DATETIME2 '$.Dropoffdate',Createddate DATETIME2 '$.Createddate'
			)) AS source
			ON target.Parcelid = source.Parcelid
			WHEN MATCHED THEN
			UPDATE SET target.Senderid = source.Senderid,target.Receiverid =source.Receiverid,target.Parceltypeid =source.Parceltypeid,target.Parcelweight =source.Parcelweight,target.Dimensions =source.Dimensions,target.Parcelstatusid =source.Parcelstatusid,target.Transitdays =source.Transitdays,target.DeliveryFee =source.DeliveryFee,target.Pickupdate =DATEADD(Day,source.Transitdays,JSON_VALUE(@JsonObjectData, '$.Dropoffdate')),target.Dropoffdate =source.Dropoffdate
			WHEN NOT MATCHED BY TARGET THEN
			INSERT (Senderid,Receiverid,Collectioncenterid,Parceltypeid,Parcelweight,Dimensions,Parcelstatusid,Transitdays,Deliveryfee,Trackingnumber,Pickupdate,Dropoffdate,Createddate)
			VALUES (source.Senderid,source.Receiverid,source.Collectioncenterid,source.Parceltypeid,source.Parcelweight,source.Dimensions,source.Parcelstatusid,source.Transitdays,source.Deliveryfee,'PRCL' + FORMAT(GETDATE(), 'ddMMmmss'),DATEADD(Day,source.Transitdays,JSON_VALUE(@JsonObjectData, '$.Dropoffdate')),source.Dropoffdate,source.Createddate)
		    OUTPUT inserted.Parcelid INTO @Parcels;
		    SET @Parcelid = (SELECT TOP 1 Parcelid FROM @Parcels);

			INSERT  INTO Parcelstatuses(Parcelid,Parcelstatusid,Updatedate,Updatedby)
			VALUES(@Parcelid,JSON_VALUE(@JsonObjectData, '$.Parcelstatusid'),JSON_VALUE(@JsonObjectData, '$.Createddate'),JSON_VALUE(@JsonObjectData, '$.Createdby'))

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