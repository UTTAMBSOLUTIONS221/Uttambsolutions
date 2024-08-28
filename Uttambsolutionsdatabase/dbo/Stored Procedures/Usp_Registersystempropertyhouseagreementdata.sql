CREATE PROCEDURE [dbo].[Usp_Registersystempropertyhouseagreementdata]
    @JsonObjectdata NVARCHAR(MAX)
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '',
			@Agreementid BIGINT,
			@Signatureimageurl VARCHAR(200);

    BEGIN TRY
        BEGIN TRANSACTION;
		DECLARE @Systempropertyhouseagreementdata TABLE (Agreementid BIGINT,Signatureimageurl VARCHAR(200));
		MERGE Systempropertyhouseagreements AS target
        USING (SELECT JSON_VALUE(@JsonObjectdata, '$.Agreementid') AS Agreementid,JSON_VALUE(@JsonObjectdata, '$.Propertyhouseid') AS Propertyhouseid,JSON_VALUE(@JsonObjectdata, '$.Propertyhouseowner') AS Ownerortenantid,JSON_VALUE(@JsonObjectdata, '$.Agreementname') AS Agreementname,JSON_VALUE(@JsonObjectdata, '$.Ownerortenant') AS Ownerortenant, JSON_VALUE(@JsonObjectdata, '$.Signatureimageurl') AS Signatureimageurl,JSON_VALUE(@JsonObjectdata, '$.Agreementdetailpdfurl') AS Agreementdetailpdfurl,CAST(JSON_VALUE(@JsonObjectdata, '$.Datecreated') AS DATETIME2) AS Datecreated) AS source
        ON  target.Agreementid = source.Agreementid
        WHEN MATCHED THEN UPDATE SET target.Agreementname = source.Agreementname, target.Ownerortenant = source.Ownerortenant,target.Signatureimageurl = source.Signatureimageurl,target.Agreementdetailpdfurl = source.Agreementdetailpdfurl,target.Datecreated = source.Datecreated
        WHEN NOT MATCHED THEN 
		INSERT (Propertyhouseid, Ownerortenantid, Agreementname, Ownerortenant, Signatureimageurl,Agreementdetailpdfurl, Datecreated)
        VALUES (source.Propertyhouseid, source.Ownerortenantid, source.Agreementname, source.Ownerortenant, source.Signatureimageurl,source.Agreementdetailpdfurl, source.Datecreated)
		OUTPUT inserted.Agreementid,inserted.Signatureimageurl INTO @Systempropertyhouseagreementdata;

        SET @RespMsg = 'Success.'
        SET @RespStat = 0; 
        
        COMMIT TRANSACTION;

        SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage,Agreementid AS Data1, Signatureimageurl AS Data2 FROM @Systempropertyhouseagreementdata;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT ''
        PRINT 'Error ' + error_message();
        SELECT 2 AS RespStatus, '0 - Error(s) Occurred' + error_message() AS RespMessage;
    END CATCH

    SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;
END