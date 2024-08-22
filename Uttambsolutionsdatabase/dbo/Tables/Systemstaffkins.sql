CREATE TABLE [dbo].[Systemstaffkins] (
    [Staffkinid]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [Userid]            BIGINT        NOT NULL,
    [Kinname]           VARCHAR (100) NOT NULL,
    [Kinphonenumber]    VARCHAR (14)  NOT NULL,
    [Kinrelationshipid] BIGINT        NOT NULL,
    [Datecreated]       DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Staffkinid] ASC)
);

