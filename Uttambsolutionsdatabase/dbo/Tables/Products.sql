CREATE TABLE [dbo].[Products] (
    [Productid]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [Productbarcode]     VARCHAR (40)    NOT NULL,
    [Productname]        VARCHAR (100)   NOT NULL,
    [Productdescription] VARCHAR (4000)  NOT NULL,
    [Categoryid]         INT             NOT NULL,
    [Brandid]            INT             NOT NULL,
    [Sku]                VARCHAR (100)   NOT NULL,
    [Wholesaleprice]     DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [Retailprice]        DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [Primaryimageurl]    VARCHAR (200)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Productid] ASC),
    CONSTRAINT [FK_Products_Productcategories] FOREIGN KEY ([Categoryid]) REFERENCES [dbo].[Productcategories] ([Categoryid])
);

