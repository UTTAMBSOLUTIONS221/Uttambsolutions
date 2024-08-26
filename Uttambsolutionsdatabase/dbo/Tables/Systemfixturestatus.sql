CREATE TABLE [dbo].[Systemfixturestatus] (
    [Fixturestatusid] BIGINT       IDENTITY (1, 1) NOT NULL,
    [Fixturestatus]   VARCHAR (20) NOT NULL,
    PRIMARY KEY CLUSTERED ([Fixturestatusid] ASC)
);

