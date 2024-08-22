CREATE TABLE [dbo].[Systemjobbenefit] (
    [Jobbenefitid] INT            IDENTITY (1, 1) NOT NULL,
    [Jobid]        INT            NOT NULL,
    [Jobbenefit]   NVARCHAR (200) NOT NULL,
    [Datecreated]  DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Jobbenefitid] ASC)
);

