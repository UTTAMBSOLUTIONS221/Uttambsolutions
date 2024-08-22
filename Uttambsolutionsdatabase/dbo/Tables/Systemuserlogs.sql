CREATE TABLE [dbo].[Systemuserlogs] (
    [Logid]             INT             IDENTITY (1, 1) NOT NULL,
    [Userid]            INT             NOT NULL,
    [Modulename]        VARCHAR (100)   NOT NULL,
    [Logaction]         VARCHAR (200)   NOT NULL,
    [Browser]           VARCHAR (200)   NULL,
    [Ipaddress]         VARCHAR (200)   NULL,
    [Loyaltyreward]     DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [Loyaltystatus]     INT             DEFAULT ((1)) NOT NULL,
    [Logactionexittime] INT             DEFAULT ((0)) NOT NULL,
    [Datecreated]       DATETIME        DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Logid] ASC)
);

