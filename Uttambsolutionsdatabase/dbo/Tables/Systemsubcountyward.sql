CREATE TABLE [dbo].[Systemsubcountyward] (
    [Subcountywardid]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [Subcountyid]       BIGINT        NOT NULL,
    [Subcountywardname] VARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([Subcountywardid] ASC)
);

