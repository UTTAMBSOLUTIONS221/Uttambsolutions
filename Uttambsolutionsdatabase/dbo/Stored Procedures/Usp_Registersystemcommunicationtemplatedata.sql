CREATE PROCEDURE [dbo].[Usp_Registersystemcommunicationtemplatedata]
    @JsonObjectdata NVARCHAR(MAX)
AS
BEGIN
    DECLARE 
        @RespStat int = 0,
        @RespMsg varchar(150) = '';

    BEGIN TRY
        BEGIN TRANSACTION;
        -- Use MERGE statement to either insert or update based on Templateid
        MERGE Communicationtemplates AS target
        USING (SELECT 
                    JSON_VALUE(@JsonObjectdata, '$.Templateid') AS Templateid,
                    JSON_VALUE(@JsonObjectdata, '$.Templatename') AS Templatename,
                    JSON_VALUE(@JsonObjectdata, '$.Templatesubject') AS Templatesubject,
                    JSON_VALUE(@JsonObjectdata, '$.Templatebody') AS Templatebody,
                    JSON_VALUE(@JsonObjectdata, '$.Isemailsms') AS Isemailsms
               ) AS source
        ON target.Templateid = source.Templateid
        WHEN MATCHED THEN
            UPDATE SET 
                Templatename = source.Templatename,
                Templatesubject = source.Templatesubject,
                Templatebody = source.Templatebody,
                Isemailsms = source.Isemailsms
        WHEN NOT MATCHED THEN
            INSERT (Templatename, Templatesubject, Templatebody, Isactive, Isdeleted, Isemailsms)
            VALUES (source.Templatename, source.Templatesubject, source.Templatebody, 1, 0, source.Isemailsms);

        SET @RespMsg ='Saved Successfully';
        SET @RespStat = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @RespMsg = 'Error: ' + ERROR_MESSAGE();
        SET @RespStat = 2;
    END CATCH

    SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;
END
