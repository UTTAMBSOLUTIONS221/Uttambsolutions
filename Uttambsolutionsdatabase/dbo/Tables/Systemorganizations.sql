CREATE TABLE [dbo].[Systemorganizations] (
    [Organizationid]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [Organizationowner]       BIGINT        NOT NULL,
    [Organizationname]        VARCHAR (100) NOT NULL,
    [OrganizationPhone]       VARCHAR (17)  NOT NULL,
    [OrganizationEmail]       VARCHAR (140) NOT NULL,
    [Organizationdescription] VARCHAR (100) NOT NULL,
    [Organizationlogo]        VARCHAR (200) NOT NULL,
    [Organizationtypeid]      INT           NOT NULL,
    [Organizationstatus]      INT           NOT NULL,
    [Datecreated]             DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Organizationid] ASC)
);

