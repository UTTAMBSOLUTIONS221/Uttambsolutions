CREATE PROCEDURE [dbo].[Usp_Updatestaffcurriculumdata]
    @JsonObjectdata NVARCHAR(MAX)
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '';

    BEGIN TRY
        BEGIN TRANSACTION;
		UPDATE Systemstaffs SET Usercurriculumvitae = JSON_VALUE(@JsonObjectdata, '$.Curriculumfile') WHERE Userid =JSON_VALUE(@JsonObjectdata, '$.Userid')
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
