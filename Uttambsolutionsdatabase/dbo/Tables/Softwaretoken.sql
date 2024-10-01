CREATE TABLE [dbo].[Softwaretoken] (
    [Tokenid]     INT             IDENTITY (1, 1) NOT NULL,
    [Tokenname]   VARCHAR (100)   NOT NULL,
    [Tokenprice]  DECIMAL (10, 2) NOT NULL,
    [Totalsupply] DECIMAL (10, 2) NOT NULL,
    [Totalvalue]  DECIMAL (18, 2) NOT NULL,
    [Datecreated] DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([Tokenid] ASC),
    UNIQUE NONCLUSTERED ([Tokenname] ASC)
);

