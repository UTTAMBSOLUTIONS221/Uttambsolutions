CREATE PROCEDURE [dbo].[Usp_Registerpropertyhouseroommeterdata]
    @JsonObjectdata NVARCHAR(MAX)
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '';

    BEGIN TRY
        BEGIN TRANSACTION;
		INSERT INTO Systempropertyhouseroommeters(Systempropertyhouseroomid,Systempropertyhouseroommeternumber,Openingmeter,Movedmeter,Closingmeter,Consumedamount,Createdby,Datecreated)
		SELECT JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroomid'),JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroommeternumber'),JSON_VALUE(@JsonObjectdata, '$.Openingmeter'),JSON_VALUE(@JsonObjectdata, '$.Movedmeter'),
		JSON_VALUE(@JsonObjectdata, '$.Closingmeter'),JSON_VALUE(@JsonObjectdata, '$.Consumedamount'),JSON_VALUE(@JsonObjectdata, '$.Createdby'),CAST(JSON_VALUE(@JsonObjectData, '$.Datecreated') AS DATETIME2) AS Datecreated

        SET @RespMsg = 'Success.'
        SET @RespStat = 0; 
        
        COMMIT TRANSACTION;

        SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT ''
        PRINT 'Error ' + error_message();
        SELECT 2 AS RespStatus, '0 - Error(s) Occurred' + error_message() AS RespMessage;
    END CATCH

    SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;
END
