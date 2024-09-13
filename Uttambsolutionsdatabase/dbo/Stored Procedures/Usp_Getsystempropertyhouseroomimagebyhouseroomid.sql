CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhouseroomimagebyhouseroomid]
@Houseroomid INT,
@Systempropertyhouseroomimagedata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		 SET @Systempropertyhouseroomimagedata= 
			(SELECT(SELECT ISNULL(SPHI.Propertyimageid,0) AS Propertyimageid,ROOM.Systempropertyhouseroomid AS Propertyhouseid,ISNULL(SPHI.Houseorroom,'HouseRoom') AS Houseorroom,SPHI.Houseorroomimageurl,SPHI.Createdby,ISNULL(SPHI.Datecreated,GETDATE()) AS Datecreated,
			 (SELECT SPHID.Propertyimageid,SPHID.Propertyhouseid,SPHID.Houseorroom,SPHID.Houseorroomimageurl,SPHID.Createdby,SPHID.Datecreated FROM Systempropertyhouseimages SPHID  FOR JSON PATH) AS PropertyHouseImage
			FROM Systempropertyhouserooms ROOM 
			LEFT JOIN Systempropertyhouseimages SPHI ON ROOM.Systempropertyhouseroomid=SPHI.Propertyhouseid
			WHERE ROOM.Systempropertyhouseroomid =@Houseroomid  AND SPHI.Houseorroom ='HouseRoom'
			FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			)AS Data
			FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			);

	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systempropertyhouseroomimagedata AS Systempropertyhouseroomimagedata;

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