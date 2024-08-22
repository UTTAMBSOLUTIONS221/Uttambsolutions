CREATE TABLE [dbo].[Systemstaffsaccount] (
    [Accountid]     BIGINT   IDENTITY (1, 1) NOT NULL,
    [Userid]        BIGINT   NOT NULL,
    [Accountnumber] BIGINT   NOT NULL,
    [Datecreated]   DATETIME NULL,
    [Datemodified]  DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Accountid] ASC),
    UNIQUE NONCLUSTERED ([Accountnumber] ASC)
);

