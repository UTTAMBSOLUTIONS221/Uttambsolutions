CREATE TABLE [dbo].[Systempropertyhouserooms] (
    [Systempropertyhouseroomid]      INT             IDENTITY (1, 1) NOT NULL,
    [Systempropertyhouseid]          INT             NOT NULL,
    [Systempropertyhousesizeid]      INT             NOT NULL,
    [Systempropertyhousesizename]    VARCHAR (100)   NOT NULL,
    [Systempropertyhousesizerent]    DECIMAL (18, 2) NOT NULL,
    [Systempropertyhousesizedeposit] BIT             NOT NULL,
    [Isvacant]                       BIT             DEFAULT ((1)) NOT NULL,
    [Isunderrenovation]              BIT             DEFAULT ((0)) NOT NULL,
    [Isshop]                         BIT             DEFAULT ((0)) NOT NULL,
    [Isgroundfloor]                  BIT             DEFAULT ((0)) NOT NULL,
    [Hasbalcony]                     BIT             DEFAULT ((0)) NOT NULL,
    [Forcaretaker]                   BIT             DEFAULT ((0)) NOT NULL,
    [Kitchentypeid]                  INT             DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Systempropertyhouseroomid] ASC)
);

