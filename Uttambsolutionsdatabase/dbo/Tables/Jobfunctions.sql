CREATE TABLE [dbo].[Jobfunctions] (
    [Jobfunctionid] INT            IDENTITY (1, 1) NOT NULL,
    [Functionname]  NVARCHAR (100) NOT NULL,
    [DateCreated]   DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Jobfunctionid] ASC)
);

