CREATE TABLE [dbo].[Systempermissions] (
    [PermissionId]   BIGINT       IDENTITY (1, 1) NOT NULL,
    [Permissionname] VARCHAR (70) NOT NULL,
    [Isactive]       BIT          CONSTRAINT [DF__Systemper__Isact__29572725] DEFAULT ((1)) NOT NULL,
    [Isdeleted]      BIT          CONSTRAINT [DF__Systemper__Isdel__2A4B4B5E] DEFAULT ((0)) NOT NULL,
    [Module]         INT          NOT NULL,
    [Isadmin]        BIT          CONSTRAINT [DF__Systemper__Isadm__2B3F6F97] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__Systempe__EFA6FB2FF4E0681D] PRIMARY KEY CLUSTERED ([PermissionId] ASC)
);

