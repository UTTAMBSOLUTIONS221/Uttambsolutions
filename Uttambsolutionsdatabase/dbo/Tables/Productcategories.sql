CREATE TABLE [dbo].[Productcategories] (
    [Categoryid]         INT            IDENTITY (1, 1) NOT NULL,
    [Categoryname]       VARCHAR (255)  NOT NULL,
    [Parentcategoryname] VARCHAR (400)  NULL,
    [Vatrate]            DECIMAL (5, 2) NULL,
    [Imageurl]           VARCHAR (255)  NULL,
    CONSTRAINT [PK__Productc__1906062329BF8316] PRIMARY KEY CLUSTERED ([Categoryid] ASC)
);

