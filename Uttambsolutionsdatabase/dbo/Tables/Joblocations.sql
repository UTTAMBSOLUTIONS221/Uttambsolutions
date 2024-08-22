CREATE TABLE [dbo].[Joblocations] (
    [Joblocationid] INT            IDENTITY (1, 1) NOT NULL,
    [Locationname]  NVARCHAR (100) NOT NULL,
    [DateCreated]   DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Joblocationid] ASC)
);

