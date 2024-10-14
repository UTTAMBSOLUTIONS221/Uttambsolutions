CREATE TABLE [dbo].[Notifications] (
    [Notificationid]      INT      IDENTITY (1, 1) NOT NULL,
    [Userid]              BIGINT   NULL,
    [Notificationmessage] TEXT     NULL,
    [Isread]              BIT      DEFAULT ((0)) NULL,
    [Createddate]         DATETIME DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Notificationid] ASC),
    FOREIGN KEY ([Userid]) REFERENCES [dbo].[Systemstaffs] ([Userid])
);

