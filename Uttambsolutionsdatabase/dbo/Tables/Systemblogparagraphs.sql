CREATE TABLE [dbo].[Systemblogparagraphs] (
    [Blogparagraphid]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Blogid]                   BIGINT         NOT NULL,
    [Blogparagraphcontent]     VARCHAR (4000) NULL,
    [Blogparagraphimageurl]    VARCHAR (200)  NULL,
    [Blogparagraphimagename]   VARCHAR (200)  NULL,
    [Blogparagraphimagesource] VARCHAR (100)  NULL,
    [Createdby]                BIGINT         NOT NULL,
    [Modifiedby]               BIGINT         NOT NULL,
    [Datecreated]              DATETIME       NOT NULL,
    [Datemodified]             DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Blogparagraphid] ASC)
);

