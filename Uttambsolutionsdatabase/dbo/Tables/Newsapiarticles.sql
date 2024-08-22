CREATE TABLE [dbo].[Newsapiarticles] (
    [SourceId]    NVARCHAR (MAX) NULL,
    [SourceName]  NVARCHAR (MAX) NULL,
    [Author]      NVARCHAR (MAX) NULL,
    [Title]       NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Url]         NVARCHAR (MAX) NULL,
    [UrlToImage]  NVARCHAR (MAX) NULL,
    [PublishedAt] DATETIME2 (7)  NULL,
    [Content]     NVARCHAR (MAX) NULL
);

