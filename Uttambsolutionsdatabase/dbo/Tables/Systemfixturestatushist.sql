CREATE TABLE [dbo].[Systemfixturestatushist] (
    [Fixturestatushistid] BIGINT IDENTITY (1, 1) NOT NULL,
    [Propertychecklistid] BIGINT NOT NULL,
    [Fixturestatusid]     BIGINT NOT NULL,
    [Fixtureunits]        INT    NOT NULL,
    PRIMARY KEY CLUSTERED ([Fixturestatushistid] ASC)
);

