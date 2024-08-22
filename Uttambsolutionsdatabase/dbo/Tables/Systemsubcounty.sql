CREATE TABLE [dbo].[Systemsubcounty] (
    [Subcountyid]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [Countyid]      BIGINT        NOT NULL,
    [Subcountyname] VARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([Subcountyid] ASC)
);

