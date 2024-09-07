CREATE TABLE [dbo].[Systemuserdevices] (
    [Systemuserdeviceid] INT          IDENTITY (1, 1) NOT NULL,
    [Userid]             BIGINT       DEFAULT ((0)) NOT NULL,
    [Androidid]          VARCHAR (20) NOT NULL,
    [Manufacturer]       VARCHAR (20) NOT NULL,
    [Model]              VARCHAR (20) NOT NULL,
    [Osversion]          VARCHAR (20) NOT NULL,
    [Platforms]          VARCHAR (20) NOT NULL,
    [Devicename]         VARCHAR (20) NOT NULL,
    [Datecreated]        DATETIME     NOT NULL,
    [Datemodified]       DATETIME     NOT NULL,
    PRIMARY KEY CLUSTERED ([Systemuserdeviceid] ASC)
);

