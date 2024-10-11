CREATE PROCEDURE [dbo].[Usp_Registersystemblogdata]
    @JsonObjectdata VARCHAR(MAX)
AS
BEGIN
    DECLARE @RespStat int = 0,
            @RespMsg varchar(150) = '',
            @Blogid BIGINT = NULL;

    BEGIN TRY
        -- Validate
        BEGIN TRANSACTION;

        DECLARE @Systemblogdata TABLE (Blogid BIGINT);

        -- Merge into Systemblogs and capture inserted ID
        MERGE INTO Systemblogs AS target
        USING (
            SELECT 
                Blogid,
                Blogcategoryid,
                Blogname,
                Blogcontent,
                Summary,
                Blogprimaryimageurl,
				Blogimagename,
				Blogimagesource,
                Blogtags,
                Blogowner,
                IsPublished,
                Blogstatus,
                Createdby,
                Modifiedby,
                Datecreated,
                Datemodified
            FROM OPENJSON(@JsonObjectdata)
            WITH (
                Blogid BIGINT '$.Blogid',
                Blogcategoryid BIGINT '$.Blogcategoryid',
                Blogname VARCHAR(100) '$.Blogname',
                Blogcontent VARCHAR(MAX) '$.Blogcontent',
                Summary VARCHAR(MAX) '$.Summary',
                Blogprimaryimageurl VARCHAR(255) '$.Blogprimaryimageurl',
				Blogimagename VARCHAR(100) '$.Blogimagename',
				Blogimagesource VARCHAR(70) '$.Blogimagesource',
                Blogtags VARCHAR(255) '$.Blogtags',
                Blogowner VARCHAR(50) '$.Blogowner',
                IsPublished BIT '$.IsPublished',
                Blogstatus VARCHAR(50) '$.Blogstatus',
                Createdby VARCHAR(50) '$.Createdby',
                Modifiedby VARCHAR(50) '$.Modifiedby',
                Datecreated DATETIME2 '$.Datecreated',
                Datemodified DATETIME2 '$.Datemodified'
            )
        ) AS source
        ON target.Blogid = source.Blogid
        WHEN MATCHED THEN
            UPDATE SET 
                target.Blogcategoryid = source.Blogcategoryid,
                target.Blogname = source.Blogname,
                target.Blogcontent = source.Blogcontent,
                target.Summary = source.Summary,
				target.Blogimagename = source.Blogimagename,
				target.Blogprimaryimageurl = source.Blogprimaryimageurl,
				target.Blogimagesource = source.Blogimagesource,
                target.Blogtags = source.Blogtags,
                target.Modifiedby = source.Modifiedby,
                target.Datemodified = source.Datemodified
        WHEN NOT MATCHED BY TARGET THEN
            INSERT (Blogcategoryid, Blogname, Blogcontent, Summary, Blogprimaryimageurl,Blogimagename,Blogimagesource, Blogtags, Blogowner, IsPublished, Blogstatus, Createdby, Modifiedby, Datecreated, Datemodified)
            VALUES (source.Blogcategoryid, source.Blogname, source.Blogcontent, source.Summary, source.Blogprimaryimageurl,source.Blogimagename,source.Blogimagesource, source.Blogtags, source.Blogowner, source.IsPublished, source.Blogstatus, source.Createdby, source.Modifiedby, source.Datecreated, source.Datemodified)
            OUTPUT inserted.Blogid INTO @Systemblogdata;

        -- Set @Blogid to the new Blogid if an insert occurred
        SET @Blogid = (SELECT TOP 1 Blogid FROM @Systemblogdata);

        -- Insert into Systemblogparagraphs
        MERGE INTO Systemblogparagraphs AS target
        USING (
            SELECT 
                Blogparagraphid,
                ISNULL(@Blogid, Blogid) AS Blogid,
                Blogparagraphcontent,
                Blogparagraphimageurl,
				Blogparagraphimagename,
				Blogparagraphimagesource,
                Createdby,
                Modifiedby,
                Datecreated,
                Datemodified
            FROM OPENJSON(@JsonObjectdata, '$.Systemblogparagraph')
            WITH (
                Blogparagraphid BIGINT '$.Blogparagraphid',
                Blogid BIGINT '$.Blogid',
                Blogparagraphcontent VARCHAR(MAX) '$.Blogparagraphcontent',
                Blogparagraphimageurl VARCHAR(255) '$.Blogparagraphimageurl',
				Blogparagraphimagename VARCHAR(100) '$.Blogparagraphimagename',
			    Blogparagraphimagesource VARCHAR(70) '$.Blogparagraphimagesource',
                Createdby VARCHAR(50) '$.Createdby',
                Modifiedby VARCHAR(50) '$.Modifiedby',
                Datecreated DATETIME2 '$.Datecreated',
                Datemodified DATETIME2 '$.Datemodified'
            )
        ) AS source
        ON target.Blogparagraphid = source.Blogparagraphid
        WHEN MATCHED THEN
            UPDATE SET 
                target.Blogparagraphcontent = source.Blogparagraphcontent,target.Blogparagraphimagename = source.Blogparagraphimagename,
				target.Blogparagraphimagesource = source.Blogparagraphimagesource,
                target.Modifiedby = source.Modifiedby,
                target.Datemodified = source.Datemodified
        WHEN NOT MATCHED THEN
            INSERT (Blogid, Blogparagraphcontent, Blogparagraphimageurl, Blogparagraphimagename,Blogparagraphimagesource,Createdby, Modifiedby, Datecreated, Datemodified)
            VALUES (source.Blogid, source.Blogparagraphcontent, source.Blogparagraphimageurl,source.Blogparagraphimagename, source.Blogparagraphimagesource,source.Createdby, source.Modifiedby, source.Datecreated, source.Datemodified);

        SET @RespMsg = 'Success';
        SET @RespStat = 0;

        COMMIT TRANSACTION;
        SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT 'Error: ' + ERROR_MESSAGE();
        SELECT 2 AS RespStatus, '0 - Error(s) Occurred: ' + ERROR_MESSAGE() AS RespMessage;
    END CATCH;

    SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;
    RETURN;
END