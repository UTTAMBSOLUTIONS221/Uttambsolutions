CREATE TABLE [dbo].[EmailLogs] (
    [EmailLogId]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [ModuleId]     BIGINT        NOT NULL,
    [EmailAddress] VARCHAR (150) NOT NULL,
    [EmailSubject] VARCHAR (400) NOT NULL,
    [EmailMessage] VARCHAR (MAX) NOT NULL,
    [IsEmailSent]  BIT           DEFAULT ((0)) NOT NULL,
    [DateTimeSent] DATETIME      DEFAULT (getdate()) NOT NULL,
    [Datecreated]  DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([EmailLogId] ASC)
);

