CREATE TABLE [dbo].[Systemblogcategories] (
    [Blogcategoryid]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [Blogcategoryname] VARCHAR (100) NOT NULL,
    [Datecreated]      DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Blogcategoryid] ASC)
);

