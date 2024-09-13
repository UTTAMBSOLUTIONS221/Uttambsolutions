CREATE PROCEDURE [dbo].[Usp_Registersystempropertyhouseroommeterdata]
@JsonObjectdata VARCHAR(MAX)
AS
BEGIN
   BEGIN
	DECLARE @RespStat int = 0,
			@RespMsg varchar(150) = '',
			@Systempropertyhouseroomid  bigint;
		  
	BEGIN
		BEGIN TRY	
		--Validate
	

		BEGIN TRANSACTION;
		    INSERT INTO Systempropertyhouseroommeters (Systempropertyhouseroomid, Systempropertyhouseroommeternumber, Openingmeter, Movedmeter, Closingmeter, Consumedamount, Createdby, Datecreated)
			SELECT JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroomid') AS Systempropertyhouseroomid,JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroommeternumber') AS Systempropertyhouseroommeternumber,TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Openingmeter') AS FLOAT) AS Openingmeter,TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Movedmeter') AS FLOAT) AS Movedmeter,TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Closingmeter') AS FLOAT) AS Closingmeter,TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Consumedamount') AS FLOAT) AS Consumedamount,JSON_VALUE(@JsonObjectdata, '$.Createdby') AS Createdby,TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Datecreated') AS DATETIME2) AS Datecreated
			WHERE TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Closingmeter') AS FLOAT) > 0 OR TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Movedmeter') AS FLOAT) > 0 OR TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Consumedamount') AS FLOAT) > 0;

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