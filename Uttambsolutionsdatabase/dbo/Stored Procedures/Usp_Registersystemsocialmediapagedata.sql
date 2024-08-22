CREATE PROCEDURE [dbo].[Usp_Registersystemsocialmediapagedata]
    @JsonObjectdata VARCHAR(MAX)
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '';

    BEGIN TRY
        -- Begin transaction
        BEGIN TRANSACTION;

        -- Perform the MERGE operation
        MERGE INTO Socialmediasettings AS target
        USING (
            SELECT Socialsettingid, Socialowner, Socialpagename, Appid, Appsecret, Useraccesstoken, 
                   Pageaccesstoken, Pageid,Pagetype, Extra, Extra1, Extra2, Extra3, Extra4,
                   Extra5, Extra6, Extra7, Extra8, Extra9, Extra10, Createdby, Modifiedby,
                   Datecreated, Datemodified
            FROM OPENJSON(@JsonObjectdata)
            WITH (
                Socialsettingid BIGINT '$.Socialsettingid',
                Socialowner BIGINT '$.SocialOwner',
                Socialpagename VARCHAR(70) '$.Socialpagename',
                Appid VARCHAR(200) '$.Appid',
                Appsecret VARCHAR(200) '$.Appsecret',
                Useraccesstoken VARCHAR(200) '$.UserAccessToken',
                Pageaccesstoken VARCHAR(200) '$.PageAccessToken',
                Pageid VARCHAR(200) '$.PageId',
				Pagetype VARCHAR(200) '$.PageType',
                Extra VARCHAR(200) '$.Extra',
                Extra1 VARCHAR(200) '$.Extra1',
                Extra2 VARCHAR(200) '$.Extra2',
                Extra3 VARCHAR(200) '$.Extra3',
                Extra4 VARCHAR(200) '$.Extra4',
                Extra5 VARCHAR(200) '$.Extra5',
                Extra6 VARCHAR(200) '$.Extra6',
                Extra7 VARCHAR(200) '$.Extra7',
                Extra8 VARCHAR(200) '$.Extra8',
                Extra9 VARCHAR(200) '$.Extra9',
                Extra10 VARCHAR(200) '$.Extra10',
                Createdby BIGINT '$.CreatedBy',
                Modifiedby BIGINT '$.ModifiedBy',
                Datecreated DATETIME '$.DateCreated',
                Datemodified DATETIME '$.DateModified'
            )
        ) AS source
        ON target.Socialsettingid = source.Socialsettingid
        WHEN MATCHED THEN
            UPDATE SET 
                target.Socialowner = source.Socialowner,
                target.Socialpagename = source.Socialpagename,
                target.Appid = source.Appid,
                target.Appsecret = source.Appsecret,
                target.Useraccesstoken = source.Useraccesstoken,
                target.Pageaccesstoken = source.Pageaccesstoken,
                target.Pageid = source.Pageid,
				target.Pagetype = source.Pagetype,
                target.Extra = source.Extra,
                target.Extra1 = source.Extra1,
                target.Extra2 = source.Extra2,
                target.Extra3 = source.Extra3,
                target.Extra4 = source.Extra4,
                target.Extra5 = source.Extra5,
                target.Extra6 = source.Extra6,
                target.Extra7 = source.Extra7,
                target.Extra8 = source.Extra8,
                target.Extra9 = source.Extra9,
                target.Extra10 = source.Extra10,
                target.Modifiedby = source.Modifiedby,
                target.Datemodified = source.Datemodified
        WHEN NOT MATCHED THEN
            INSERT (Socialowner, Socialpagename, Appid, Appsecret, Useraccesstoken,
                    Pageaccesstoken, Pageid,Pagetype, Extra, Extra1, Extra2, Extra3,
                    Extra4, Extra5, Extra6, Extra7, Extra8, Extra9, Extra10,
                    Createdby, Modifiedby, Datecreated, Datemodified)
            VALUES (source.Socialowner, source.Socialpagename, source.Appid, 
                    source.Appsecret, source.Useraccesstoken, source.Pageaccesstoken, 
                    source.Pageid,source.Pagetype, source.Extra, source.Extra1, source.Extra2,
                    source.Extra3, source.Extra4, source.Extra5, source.Extra6,
                    source.Extra7, source.Extra8, source.Extra9, source.Extra10,
                    source.Createdby, source.Modifiedby, source.Datecreated, 
                    source.Datemodified);

        SET @RespMsg = 'Success';
        SET @RespStat = 0; 

        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @RespStat = 2;
        SET @RespMsg = '0 - Error(s) Occurred: ' + ERROR_MESSAGE();
    END CATCH

    SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;
    RETURN; 
END
