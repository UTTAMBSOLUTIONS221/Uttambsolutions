CREATE TABLE [dbo].[Tokenownership] (
    [Tokenownershipid] INT             IDENTITY (1, 1) NOT NULL,
    [Tokenid]          INT             NOT NULL,
    [Userid]           INT             NOT NULL,
    [Tokenamount]      DECIMAL (10, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([Tokenownershipid] ASC)
);

