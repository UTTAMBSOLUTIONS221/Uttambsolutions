CREATE TABLE [dbo].[Shopproductimages] (
    [Productimagesid] BIGINT        IDENTITY (1, 1) NOT NULL,
    [Shopproductid]   BIGINT        NOT NULL,
    [Productimageurl] VARCHAR (255) NULL,
    [DateCreated]     DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Productimagesid] ASC)
);

