CREATE TABLE [dbo].[Systemmaritalstatus] (
    [Maritalstatusid]   BIGINT       IDENTITY (1, 1) NOT NULL,
    [Maritalstatusname] VARCHAR (40) NOT NULL,
    PRIMARY KEY CLUSTERED ([Maritalstatusid] ASC)
);

