CREATE TABLE [dbo].[Systemjobqualification] (
    [Jobqualificationid] INT            IDENTITY (1, 1) NOT NULL,
    [Jobid]              INT            NOT NULL,
    [Jobqualification]   NVARCHAR (200) NOT NULL,
    [Datecreated]        DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Jobqualificationid] ASC)
);

