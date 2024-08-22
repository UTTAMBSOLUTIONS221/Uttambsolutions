CREATE TABLE [dbo].[Systemsupportedbanks] (
    [Systembankid]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [Systembankname]           VARCHAR (60)  NOT NULL,
    [Systembankpaybill]        INT           NOT NULL,
    [Systembankbaseurl]        VARCHAR (100) NULL,
    [Systembankendpointurl]    VARCHAR (100) NULL,
    [Systembankconsumerkey]    VARCHAR (100) NULL,
    [Systembankconsumersecret] VARCHAR (100) NULL,
    [Extra]                    VARCHAR (100) NULL,
    [Extra1]                   VARCHAR (100) NULL,
    [Extra2]                   VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Systembankid] ASC)
);

