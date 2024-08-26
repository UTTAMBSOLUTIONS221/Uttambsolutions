CREATE PROCEDURE [dbo].[Usp_Registersystempropertyhouseagreementdata]
    @JsonObjectdata NVARCHAR(MAX)
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '';

    BEGIN TRY
        BEGIN TRANSACTION;
		INSERT INTO Systempropertyhouseagreements(Propertyhouseid,Ownerortenantid,Agreementname,Ownerortenant,Signatureimageurl,Datecreated)
		SELECT JSON_VALUE(@JsonObjectdata, '$.Propertyhouseid'),JSON_VALUE(@JsonObjectdata, '$.Propertyhouseowner'),JSON_VALUE(@JsonObjectdata, '$.Agreementname'),JSON_VALUE(@JsonObjectdata, '$.Ownerortenant'),
		JSON_VALUE(@JsonObjectdata, '$.Signatureimageurl'),CAST(JSON_VALUE(@JsonObjectData, '$.Datecreated') AS DATETIME2) AS Datecreated

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