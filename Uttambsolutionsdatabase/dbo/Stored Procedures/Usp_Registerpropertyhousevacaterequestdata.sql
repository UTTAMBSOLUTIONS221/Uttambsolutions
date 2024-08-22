CREATE PROCEDURE [dbo].[Usp_Registerpropertyhousevacaterequestdata]
    @JsonObjectdata NVARCHAR(MAX)
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '';

    BEGIN TRY
        BEGIN TRANSACTION;
		INSERT INTO Systempropertyhousevacatingrequests(Systempropertyhousetenantid,Systempropertyhouseroomid,Plannedvacatingdate,Expectedvacatingdate,Vacatingreason,Vacatingstatus,Approvedby,Datecreated,Dateapproved)
		SELECT JSON_VALUE(@JsonObjectdata, '$.Systempropertyhousetenantid'),JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroomid'),
		CAST(JSON_VALUE(@JsonObjectData, '$.Plannedvacatingdate') AS DATETIME2) AS Plannedvacatingdate,CAST(JSON_VALUE(@JsonObjectData, '$.Expectedvacatingdate') AS DATETIME2) AS Expectedvacatingdate,
		JSON_VALUE(@JsonObjectdata, '$.Vacatingreason'),JSON_VALUE(@JsonObjectdata, '$.Vacatingstatus'),
		JSON_VALUE(@JsonObjectdata, '$.Approvedby'),CAST(JSON_VALUE(@JsonObjectData, '$.Datecreated') AS DATETIME2) AS Datecreated,CAST(JSON_VALUE(@JsonObjectData, '$.Dateapproved') AS DATETIME2) AS Dateapproved

		UPDATE Systempropertyhouseroomstenant SET Occupationalstatus = 1 WHERE Systempropertyhousetenantid= JSON_VALUE(@JsonObjectdata, '$.Systempropertyhousetenantid') AND Systempropertyhouseroomid= JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroomid');


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
