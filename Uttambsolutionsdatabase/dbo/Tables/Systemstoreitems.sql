CREATE TABLE [dbo].[Systemstoreitems] (
    [Storeitemid]      INT             IDENTITY (1, 1) NOT NULL,
    [Storeitemname]    VARCHAR (100)   NOT NULL,
    [Itembrandname]    VARCHAR (70)    NOT NULL,
    [Itemsize]         VARCHAR (70)    NULL,
    [Itembuyingprice]  DECIMAL (10, 2) NOT NULL,
    [Itemsellingprice] DECIMAL (10, 2) NOT NULL,
    [Itemstatus]       INT             DEFAULT ((0)) NOT NULL,
    [Isactive]         BIT             DEFAULT ((1)) NOT NULL,
    [Isdeleted]        BIT             DEFAULT ((0)) NOT NULL,
    [Createdby]        BIGINT          NOT NULL,
    [Modifiedby]       BIGINT          NOT NULL,
    [Datecreated]      DATETIME        NOT NULL,
    [Datemodified]     DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([Storeitemid] ASC)
);

