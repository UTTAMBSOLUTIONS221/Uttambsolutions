CREATE TABLE [dbo].[Systempropertyhouseroomstenant] (
    [Systempropertyhousetenantentryid] BIGINT          IDENTITY (1, 1) NOT NULL,
    [Systempropertyhousetenantid]      BIGINT          NOT NULL,
    [Systempropertyhouseroomid]        INT             NOT NULL,
    [Isoccupant]                       BIT             DEFAULT ((0)) NOT NULL,
    [Occupationalstatus]               INT             NOT NULL,
    [Roomoccupant]                     INT             DEFAULT ((0)) NOT NULL,
    [Roomoccupantdetail]               VARCHAR (200)   NOT NULL,
    [Isnewtenant]                      BIT             DEFAULT ((0)) NOT NULL,
    [Ratingvalue]                      DECIMAL (10, 2) DEFAULT ((0)) NOT NULL,
    [Ownercomment]                     VARCHAR (200)   DEFAULT ('Not Set') NOT NULL,
    [Createdby]                        INT             NOT NULL,
    [Modifiedby]                       INT             NOT NULL,
    [Vacateddate]                      DATETIME        NOT NULL,
    [Datecreated]                      DATETIME        NOT NULL,
    [Datemodified]                     DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([Systempropertyhousetenantentryid] ASC)
);






GO
