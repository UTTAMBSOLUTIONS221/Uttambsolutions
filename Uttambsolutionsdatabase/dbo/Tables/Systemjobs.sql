﻿CREATE TABLE [dbo].[Systemjobs] (
    [JobId]           INT             IDENTITY (1, 1) NOT NULL,
    [EmployerId]      BIGINT          NOT NULL,
    [Title]           NVARCHAR (400)  NOT NULL,
    [Jobdescription]  NVARCHAR (1000) NULL,
    [Jobfunctionid]   INT             NULL,
    [Jobindustryid]   INT             NULL,
    [Joblocationid]   INT             NULL,
    [Jobtypeid]       INT             NULL,
    [Jobexperienceid] INT             NULL,
    [Jobsalaryrange]  NVARCHAR (100)  NULL,
    [Deadline]        DATETIME        NULL,
    [Jobstatus]       NVARCHAR (50)   DEFAULT ('Open') NULL,
    [Dateposted]      DATETIME        DEFAULT (getdate()) NULL,
    [Joburl]          VARCHAR (500)   NULL,
    [Easyapply]       BIT             DEFAULT ((0)) NULL,
    [Hasatest]        BIT             DEFAULT ((0)) NULL,
    [Isfeatured]      BIT             DEFAULT ((0)) NULL,
    [Approved]        BIT             DEFAULT ((0)) NULL,
    [Ispublished]     BIT             DEFAULT ((0)) NULL,
    [Jobreportto]     VARCHAR (200)   NULL,
    [Jobimageburl]    VARCHAR (240)   NULL,
    [Jobhowtoapply]   VARCHAR (1000)  NULL,
    [Createdby]       BIGINT          NOT NULL,
    [Datecreated]     DATETIME        DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([JobId] ASC)
);

