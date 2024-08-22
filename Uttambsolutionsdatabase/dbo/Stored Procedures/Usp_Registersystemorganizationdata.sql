CREATE PROCEDURE [dbo].[Usp_Registersystemorganizationdata]
@JsonObjectdata VARCHAR(MAX)
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '';

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validate and merge data
        MERGE INTO Systemorganizations AS target
        USING (
            SELECT Organizationid, Organizationowner, Organizationname,OrganizationPhone,OrganizationEmail, Organizationdescription,Organizationlogo, Organizationtypeid, Organizationstatus, Datecreated
            FROM OPENJSON(@JsonObjectdata)
            WITH (
                Organizationid BIGINT '$.OrganizationId',
                Organizationowner BIGINT '$.OrganizationOwner',
                Organizationname VARCHAR(100) '$.OrganizationName',
				OrganizationPhone VARCHAR(17) '$.OrganizationPhone',
				OrganizationEmail VARCHAR(140) '$.OrganizationEmail',
                Organizationdescription VARCHAR(100) '$.OrganizationDescription',
				Organizationlogo VARCHAR(200) '$.OrganizationLogo',
                Organizationtypeid INT '$.OrganizationTypeId',
                Organizationstatus INT '$.OrganizationStatus',
                Datecreated DATETIME '$.DateCreated'
            )
        ) AS source
        ON target.Organizationid = source.Organizationid
        WHEN MATCHED THEN
            UPDATE SET 
                target.Organizationowner = source.Organizationowner,
                target.Organizationname = source.Organizationname,
				target.OrganizationPhone = source.OrganizationPhone,
				target.OrganizationEmail = source.OrganizationEmail,
                target.Organizationdescription = source.Organizationdescription,
				target.Organizationlogo = source.Organizationlogo,
                target.Organizationtypeid = source.Organizationtypeid,
                target.Organizationstatus = source.Organizationstatus,
                target.Datecreated = source.Datecreated
        WHEN NOT MATCHED BY TARGET THEN
            INSERT (Organizationowner, Organizationname, OrganizationPhone,OrganizationEmail,Organizationdescription,Organizationlogo, Organizationtypeid, Organizationstatus, Datecreated)
            VALUES (source.Organizationowner, source.Organizationname,source.OrganizationPhone,source.OrganizationEmail, source.Organizationdescription,source.Organizationlogo, source.Organizationtypeid, source.Organizationstatus, source.Datecreated);

        SET @RespMsg = 'Success';
        SET @RespStat = 0;

        COMMIT TRANSACTION;
        SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT 'Error ' + ERROR_MESSAGE();
        SELECT 2 AS RespStatus, 'Error(s) Occurred: ' + ERROR_MESSAGE() AS RespMessage;
    END CATCH
END;
