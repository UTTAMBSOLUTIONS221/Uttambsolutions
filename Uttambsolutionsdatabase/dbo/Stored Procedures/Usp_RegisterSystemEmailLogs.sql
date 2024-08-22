CREATE PROCEDURE [dbo].[Usp_RegisterSystemEmailLogs]
    @JsonObjectdata NVARCHAR(MAX)
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '',
			@EmailLogId BIGINT;

    BEGIN TRY
        BEGIN TRANSACTION;

        MERGE INTO EmailLogs AS target
        USING (SELECT EmailLogId = JSON_VALUE(@JsonObjectdata, '$.EmailLogId'),ModuleId = JSON_VALUE(@JsonObjectdata, '$.ModuleId'),EmailAddress = JSON_VALUE(@JsonObjectdata, '$.EmailAddress'),EmailSubject = JSON_VALUE(@JsonObjectdata, '$.EmailSubject'),EmailMessage = JSON_VALUE(@JsonObjectdata, '$.EmailMessage'),IsEmailSent = JSON_VALUE(@JsonObjectdata, '$.IsEmailSent')
        ) AS source ON target.EmailLogId = source.EmailLogId
        WHEN MATCHED THEN
        UPDATE SET target.ModuleId = source.ModuleId,target.EmailAddress = source.EmailAddress,target.EmailSubject = source.EmailSubject,target.EmailMessage = source.EmailMessage,target.IsEmailSent = source.IsEmailSent,target.DateTimeSent = CAST(JSON_VALUE(@JsonObjectdata, '$.DateTimeSent')  AS datetime2)
        WHEN NOT MATCHED THEN
        INSERT (ModuleId, EmailAddress, EmailSubject, EmailMessage, IsEmailSent, DateTimeSent, Datecreated)
        VALUES (source.ModuleId, source.EmailAddress, source.EmailSubject, source.EmailMessage, source.IsEmailSent, CAST(JSON_VALUE(@JsonObjectdata, '$.DateTimeSent')  AS datetime2), CAST(JSON_VALUE(@JsonObjectdata, '$.Datecreated')  AS datetime2));
		SET @EmailLogId=SCOPE_IDENTITY();
        SET @RespMsg = 'Success.'
        SET @RespStat = 0; 
        
        COMMIT TRANSACTION;

        SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage,@EmailLogId AS Data1;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT ''
        PRINT 'Error ' + error_message();
        SELECT 2 AS RespStatus, '0 - Error(s) Occurred' + error_message() AS RespMessage;
    END CATCH

    SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;
END
