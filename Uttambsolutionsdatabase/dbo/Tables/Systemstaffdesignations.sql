CREATE TABLE [dbo].[Systemstaffdesignations] (
    [Staffdesignationid] BIGINT       IDENTITY (1, 1) NOT NULL,
    [Systemstaffid]      BIGINT       NOT NULL,
    [Staffdesignation]   VARCHAR (40) NOT NULL,
    [Datecreated]        DATETIME     DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Staffdesignationid] ASC)
);

