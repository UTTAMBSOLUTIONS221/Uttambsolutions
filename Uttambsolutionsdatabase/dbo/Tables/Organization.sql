CREATE TABLE [dbo].[Organization] (
    [OrganizationId]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantId]          BIGINT        NOT NULL,
    [OrganizationName]  VARCHAR (100) NOT NULL,
    [OrganizationEmail] VARCHAR (200) NOT NULL,
    CONSTRAINT [PK__Organiza__CADB0B1208FAAE14] PRIMARY KEY CLUSTERED ([OrganizationId] ASC)
);

