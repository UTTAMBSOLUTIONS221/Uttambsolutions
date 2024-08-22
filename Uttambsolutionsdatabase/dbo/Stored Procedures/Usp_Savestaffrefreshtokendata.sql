CREATE PROCEDURE [dbo].[Usp_Savestaffrefreshtokendata]
@JsonObjectData VARCHAR(MAX)
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '';

    BEGIN TRY
        -- Validate
        BEGIN TRANSACTION;
        -- Use MERGE statement for insert/update
        MERGE INTO RefreshTokens AS target
        USING (SELECT JSON_VALUE(@JsonObjectData, '$.Refreshtokenid') AS Refreshtokenid,JSON_VALUE(@JsonObjectData, '$.Token') AS Token,JSON_VALUE(@JsonObjectData, '$.Userid') AS Userid,CAST(JSON_VALUE(@JsonObjectData, '$.Expirydate') AS DATETIME2) AS Expirydate) AS source
        ON target.Userid = source.Userid
        WHEN MATCHED THEN
            UPDATE SET Token = source.Token,Expirydate = source.Expirydate
        WHEN NOT MATCHED THEN
            INSERT (Token, Expirydate, Userid)
            VALUES (source.Token, source.Expirydate, source.Userid);
		
        SET @RespMsg = 'Success';
        SET @RespStat = 0; 
        COMMIT TRANSACTION;

        SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT '';
        PRINT 'Error ' + ERROR_MESSAGE();
        SELECT 2 AS RespStatus, '0 - Error(s) Occurred: ' + ERROR_MESSAGE() AS RespMessage;
    END CATCH

    SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;
    RETURN;
END
