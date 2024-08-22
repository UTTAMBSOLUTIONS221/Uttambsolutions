CREATE TABLE [dbo].[Systempropertyhousebenefits] (
    [Systempropertyhousebenefitid]     BIGINT IDENTITY (1, 1) NOT NULL,
    [Propertyhouseid]                  BIGINT NOT NULL,
    [Housebenefitid]                   BIGINT NOT NULL,
    [Systempropertyhousebenefitwehave] BIT    DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Systempropertyhousebenefitid] ASC),
    CONSTRAINT [FK_Systempropertyhousebenefits_Propertyhouse] FOREIGN KEY ([Propertyhouseid]) REFERENCES [dbo].[Systempropertyhouses] ([Propertyhouseid])
);

