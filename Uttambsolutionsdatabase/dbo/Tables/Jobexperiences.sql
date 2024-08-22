CREATE TABLE [dbo].[Jobexperiences] (
    [Jobexperienceid] INT            IDENTITY (1, 1) NOT NULL,
    [Experiencename]  NVARCHAR (100) NOT NULL,
    [DateCreated]     DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Jobexperienceid] ASC)
);

