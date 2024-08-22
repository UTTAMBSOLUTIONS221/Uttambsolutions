CREATE TABLE [dbo].[Systemjobskill] (
    [Jobskillid]  INT            IDENTITY (1, 1) NOT NULL,
    [Jobid]       INT            NOT NULL,
    [Jobskill]    NVARCHAR (200) NOT NULL,
    [Datecreated] DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Jobskillid] ASC)
);

