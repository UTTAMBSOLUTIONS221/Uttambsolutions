CREATE TABLE [dbo].[Systemblogs] (
    [Blogid]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [Blogcategoryid]      BIGINT         NOT NULL,
    [Blogname]            VARCHAR (400)  NOT NULL,
    [Blogcontent]         VARCHAR (4000) NOT NULL,
    [Summary]             VARCHAR (2000) NOT NULL,
    [Blogprimaryimageurl] VARCHAR (200)  NULL,
    [Blogimagename]       VARCHAR (100)  NOT NULL,
    [Blogimagesource]     VARCHAR (70)   NOT NULL,
    [Blogtags]            VARCHAR (400)  NOT NULL,
    [Blogowner]           BIGINT         NOT NULL,
    [IsPublished]         BIT            NOT NULL,
    [Blogstatus]          INT            NOT NULL,
    [Createdby]           BIGINT         NOT NULL,
    [Modifiedby]          BIGINT         NOT NULL,
    [Datecreated]         DATETIME       NOT NULL,
    [Datemodified]        DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Blogid] ASC)
);

