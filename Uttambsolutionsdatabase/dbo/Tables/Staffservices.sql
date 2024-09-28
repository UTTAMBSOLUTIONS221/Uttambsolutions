CREATE TABLE [dbo].[Staffservices] (
    [Staffserviceid]     INT           IDENTITY (1, 1) NOT NULL,
    [Staffid]            INT           NOT NULL,
    [Servicetypeid]      INT           NOT NULL,
    [Countyid]           INT           NOT NULL,
    [Subcountyid]        INT           NOT NULL,
    [Subcountywardid]    INT           NOT NULL,
    [Streedorlandmark]   VARCHAR (200) NOT NULL,
    [Contacts]           VARCHAR (100) NULL,
    [Servicedescription] VARCHAR (200) NOT NULL,
    [Servicestatus]      INT           DEFAULT ((0)) NOT NULL,
    [Isactive]           BIT           DEFAULT ((1)) NOT NULL,
    [Isdeleted]          BIT           DEFAULT ((0)) NOT NULL,
    [Datecreated]        DATETIME      DEFAULT (getdate()) NOT NULL,
    [Datemodified]       DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Staffserviceid] ASC)
);

