CREATE TABLE [dbo].[Systemroles] (
    [Roleid]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [Rolename]        VARCHAR (100) NOT NULL,
    [RoleDescription] VARCHAR (300) NULL,
    [Tenantid]        BIGINT        NOT NULL,
    [Isdefault]       BIT           CONSTRAINT [DF__Systemrol__Isdef__2E1BDC42] DEFAULT ((0)) NOT NULL,
    [Isactive]        BIT           CONSTRAINT [DF__Systemrol__Isact__2F10007B] DEFAULT ((1)) NOT NULL,
    [Isdeleted]       BIT           CONSTRAINT [DF__Systemrol__Isdel__300424B4] DEFAULT ((0)) NOT NULL,
    [Createdby]       BIGINT        NOT NULL,
    [Modifiedby]      BIGINT        NOT NULL,
    [Datemodified]    DATETIME      NOT NULL,
    [Datecreated]     DATETIME      NOT NULL,
    CONSTRAINT [PK__Systemro__8AF5CA32CC46E31F] PRIMARY KEY CLUSTERED ([Roleid] ASC)
);

