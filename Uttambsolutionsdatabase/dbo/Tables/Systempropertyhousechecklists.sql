CREATE TABLE [dbo].[Systempropertyhousechecklists] (
    [Propertychecklistid] BIGINT   IDENTITY (1, 1) NOT NULL,
    [Propertyhouseroomid] BIGINT   NOT NULL,
    [Fixtureid]           INT      NOT NULL,
    [Fixtureunits]        INT      NOT NULL,
    [Fixturestatusid]     INT      NOT NULL,
    [Createdby]           BIGINT   NOT NULL,
    [Datecreated]         DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([Propertychecklistid] ASC)
);

