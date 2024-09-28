CREATE TABLE [dbo].[Systemservicesitems] (
    [Serviceitemid]       INT           IDENTITY (1, 1) NOT NULL,
    [Serviceid]           INT           NOT NULL,
    [Serviceitemname]     VARCHAR (200) NOT NULL,
    [Serviceitemimageurl] VARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([Serviceitemid] ASC)
);

