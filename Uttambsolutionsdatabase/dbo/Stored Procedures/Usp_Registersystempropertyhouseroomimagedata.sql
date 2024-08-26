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
			MERGE INTO Systempropertyhouseimages AS target
			USING (
			SELECT Propertyimageid,Propertyhouseid,Houseorroom,HouseorroomImageurl,Createdby,Datecreated FROM OPENJSON(@JsonObjectdata)
			WITH (Propertyimageid BIGINT '$.Propertyimageid',Propertyhouseid BIGINT '$.Propertyhouseid',Houseorroom VARCHAR(20) '$.Houseorroom',HouseorroomImageurl VARCHAR(200) '$.HouseorroomImageurl',Createdby BIGINT '$.Createdby', Datecreated DATETIME2 '$.Datecreated'
			)) AS source
			ON target.Propertyimageid = source.Propertyimageid AND target.Propertyhouseid = source.Propertyhouseid
			WHEN MATCHED THEN
			UPDATE SET target.HouseorroomImageurl =source.HouseorroomImageurl
			WHEN NOT MATCHED BY TARGET THEN
			INSERT (Propertyhouseid,Houseorroom,HouseorroomImageurl,Createdby,Datecreated)
			VALUES (source.Propertyhouseid,source.Houseorroom,source.HouseorroomImageurl,source.Createdby,source.Datecreated);
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