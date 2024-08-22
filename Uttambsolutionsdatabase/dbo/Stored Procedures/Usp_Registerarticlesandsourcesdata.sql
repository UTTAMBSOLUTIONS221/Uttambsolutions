CREATE PROCEDURE [dbo].[Usp_Registerarticlesandsourcesdata]
@JsonObjectdata VARCHAR(MAX)
AS
BEGIN
   BEGIN
	DECLARE @RespStat int = 0,
			@RespMsg varchar(150) = '';
		  
	BEGIN
		BEGIN TRY	
		--Validate

		BEGIN TRANSACTION;
			INSERT INTO Newsapiarticles (SourceId, SourceName, Author, Title, Description, Url, UrlToImage, PublishedAt, Content)
    SELECT
        JSON_VALUE(articles.source, '$.Id') AS SourceId,
        JSON_VALUE(articles.source, '$.Name') AS SourceName,
        articles.author AS Author,
        articles.title AS Title,
        articles.description AS Description,
        articles.url AS Url,
        articles.urlToImage AS UrlToImage,
        TRY_CONVERT(DATETIME2, articles.publishedAt, 126) AS PublishedAt,
        articles.content AS Content
    FROM OPENJSON(@JsonObjectdata, '$.Articles') 
    WITH (
        source NVARCHAR(MAX) '$.Source' AS JSON,
        Author NVARCHAR(MAX),
        Title NVARCHAR(MAX),
        Description NVARCHAR(MAX),
        Url NVARCHAR(MAX),
        UrlToImage NVARCHAR(MAX),
        PublishedAt NVARCHAR(MAX),
        Content NVARCHAR(MAX)
    ) AS articles
    WHERE articles.urlToImage IS NOT NULL; 
		Set @RespMsg ='Success'
		Set @RespStat =0; 
		COMMIT TRANSACTION;
		Select @RespStat as RespStatus, @RespMsg as RespMessage;

		END TRY
		BEGIN CATCH
		ROLLBACK TRANSACTION
		PRINT ''
		PRINT 'Error ' + error_message();
		Select 2 as RespStatus, '0 - Error(s) Occurred' + error_message() as RespMessage
		END CATCH
		Select @RespStat as RespStatus, @RespMsg as RespMessage;
		RETURN; 
		END;
	END
END
