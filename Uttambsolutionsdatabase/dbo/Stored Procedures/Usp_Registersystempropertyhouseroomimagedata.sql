CREATE PROCEDURE [dbo].[Usp_Registersystempropertyhouseroomimagedata]
@JsonObjectdata VARCHAR(MAX)
AS
BEGIN
   BEGIN
	DECLARE @RespStat int = 0,
			@RespMsg varchar(150) = '';
		  
	BEGIN
		BEGIN TRY	
		--Validate
		BEGIN TRANSACTION;
		  INSERT INTO Systempropertyhouseimages (Propertyhouseid, Houseorroom, HouseorroomImageurl, Createdby, Datecreated)
          SELECT JSON_VALUE(@JsonObjectdata, '$.Propertyhouseid'),JSON_VALUE(@JsonObjectdata, '$.Houseorroom'),JSON_VALUE(@JsonObjectdata, '$.Houseorroomimageurl'), JSON_VALUE(@JsonObjectdata, '$.Createdby'),CAST(JSON_VALUE(@JsonObjectData, '$.Datecreated') AS DATETIME2);

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