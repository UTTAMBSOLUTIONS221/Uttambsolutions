CREATE PROCEDURE [dbo].[Usp_Registersystemuseractivitydata]
    @JsonObjectdata NVARCHAR(MAX)
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '';

    BEGIN TRY
        BEGIN TRANSACTION;
		INSERT INTO Systemuserlogs(Userid, Modulename,Logaction,Browser,Ipaddress,Loyaltyreward,Loyaltystatus,Logactionexittime,Datecreated)
		SELECT JSON_VALUE(@JsonObjectdata, '$.Userid'),JSON_VALUE(@JsonObjectdata, '$.Modulename'),JSON_VALUE(@JsonObjectdata, '$.Logaction'),JSON_VALUE(@JsonObjectdata, '$.Browser'),
		JSON_VALUE(@JsonObjectdata, '$.Ipaddress'),JSON_VALUE(@JsonObjectdata, '$.Loyaltyreward'),JSON_VALUE(@JsonObjectdata, '$.Loyaltystatus'),
		JSON_VALUE(@JsonObjectdata, '$.Logactionexittime'),CAST(JSON_VALUE(@JsonObjectData, '$.Datecreated') AS DATETIME2) AS Datecreated

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
