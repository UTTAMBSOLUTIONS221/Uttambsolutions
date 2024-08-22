CREATE TABLE [dbo].[Systemhousedepositfees] (
    [Housedepositfeeid]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [Housedepositfeename] VARCHAR (100) NOT NULL,
    [Isrecurring]         BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Housedepositfeeid] ASC)
);

