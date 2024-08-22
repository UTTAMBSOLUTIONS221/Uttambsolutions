CREATE TABLE [dbo].[Systemjobapplications] (
    [Jobapplicationid]  INT             IDENTITY (1, 1) NOT NULL,
    [Userid]            BIGINT          NOT NULL,
    [Jobid]             INT             NOT NULL,
    [Coverletter]       NVARCHAR (1000) NOT NULL,
    [Applicationstatus] INT             DEFAULT ((3)) NOT NULL,
    [Datecreated]       DATETIME        DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Jobapplicationid] ASC)
);

