CREATE TABLE [dbo].[Systemjobfunction] (
    [Jobfunctionid] INT             IDENTITY (1, 1) NOT NULL,
    [Jobid]         INT             NOT NULL,
    [Jobfunction]   NVARCHAR (2000) NOT NULL,
    [Datecreated]   DATETIME        DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Jobfunctionid] ASC)
);

