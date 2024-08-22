CREATE TABLE [dbo].[Shopproductfeature] (
    [Productfeatureid] BIGINT        IDENTITY (1, 1) NOT NULL,
    [Shopproductid]    BIGINT        NOT NULL,
    [Productfeature]   VARCHAR (255) NULL,
    [DateCreated]      DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Productfeatureid] ASC)
);

