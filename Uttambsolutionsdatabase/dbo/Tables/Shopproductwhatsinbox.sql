CREATE TABLE [dbo].[Shopproductwhatsinbox] (
    [Productwhatsinboxid]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [Shopproductid]         BIGINT        NOT NULL,
    [Productwhatsinboxitem] VARCHAR (255) NULL,
    [DateCreated]           DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Productwhatsinboxid] ASC)
);

