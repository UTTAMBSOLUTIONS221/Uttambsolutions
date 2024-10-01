CREATE TABLE [dbo].[Systemservicesstaffs] (
    [Staffserviceid]     INT           IDENTITY (1, 1) NOT NULL,
    [Staffid]            INT           NOT NULL,
    [Iswork]             BIT           DEFAULT ((0)) NOT NULL,
    [Servicetypeid]      INT           NOT NULL,
    [Countyid]           INT           NOT NULL,
    [Subcountyid]        INT           NOT NULL,
    [Subcountywardid]    INT           NOT NULL,
    [Streedorlandmark]   VARCHAR (200) NULL,
    [Contacts]           VARCHAR (100) NOT NULL,
    [Servicedescription] VARCHAR (400) NULL,
    [Servicestatus]      INT           DEFAULT ((1)) NOT NULL,
    [Isactive]           BIT           DEFAULT ((1)) NOT NULL,
    [Isdeleted]          BIT           DEFAULT ((0)) NOT NULL,
    [Datecreated]        DATETIME      NOT NULL,
    [Datemodified]       DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Staffserviceid] ASC)
);

