CREATE TABLE [dbo].[Parcelcollectioncenters] (
    [Collectioncenterid] INT           IDENTITY (1, 1) NOT NULL,
    [Collectionname]     VARCHAR (100) NOT NULL,
    [Collectionaddress]  VARCHAR (255) NOT NULL,
    [Phonenumber]        VARCHAR (15)  NULL,
    [Operatinghours]     VARCHAR (50)  NULL,
    [Collectionstatus]   INT           DEFAULT ((0)) NOT NULL,
    [Managerid]          BIGINT        NULL,
    PRIMARY KEY CLUSTERED ([Collectioncenterid] ASC),
    FOREIGN KEY ([Managerid]) REFERENCES [dbo].[Systemstaffs] ([Userid])
);

