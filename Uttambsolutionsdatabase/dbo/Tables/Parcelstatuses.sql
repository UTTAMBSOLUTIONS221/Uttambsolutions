CREATE TABLE [dbo].[Parcelstatuses] (
    [Statusid]       INT      IDENTITY (1, 1) NOT NULL,
    [Parcelid]       INT      NULL,
    [Parcelstatusid] INT      NULL,
    [Updatedate]     DATETIME DEFAULT (getdate()) NULL,
    [Updatedby]      BIGINT   NULL,
    PRIMARY KEY CLUSTERED ([Statusid] ASC),
    FOREIGN KEY ([Parcelid]) REFERENCES [dbo].[Parcels] ([Parcelid]),
    FOREIGN KEY ([Parcelstatusid]) REFERENCES [dbo].[Parcelstatus] ([Parcelstatusid]),
    FOREIGN KEY ([Updatedby]) REFERENCES [dbo].[Systemstaffs] ([Userid])
);

