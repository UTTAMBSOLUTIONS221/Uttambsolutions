CREATE TABLE [dbo].[Productbrand] (
    [Brandid]      INT           IDENTITY (1, 1) NOT NULL,
    [Brandname]    VARCHAR (100) NOT NULL,
    [Brandpathurl] VARCHAR (240) NOT NULL,
    PRIMARY KEY CLUSTERED ([Brandid] ASC)
);

