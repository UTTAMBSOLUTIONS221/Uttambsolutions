CREATE TABLE [dbo].[Systemstafftermandservices] (
    [Stafftermandserviceid] BIGINT   IDENTITY (1, 1) NOT NULL,
    [Systemstaffid]         BIGINT   NOT NULL,
    [Accepttermandservices] BIT      DEFAULT ((1)) NOT NULL,
    [Datecreated]           DATETIME DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Stafftermandserviceid] ASC)
);

