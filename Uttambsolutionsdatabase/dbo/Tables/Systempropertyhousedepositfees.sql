CREATE TABLE [dbo].[Systempropertyhousedepositfees] (
    [Systempropertyhousedepositfeeid]         BIGINT          IDENTITY (1, 1) NOT NULL,
    [Propertyhouseid]                         BIGINT          NOT NULL,
    [Housedepositfeeid]                       BIGINT          NOT NULL,
    [Systempropertyhousedepositfeeamount]     DECIMAL (18, 2) NOT NULL,
    [Systempropertyhousesizedepositfeewehave] BIT             DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Systempropertyhousedepositfeeid] ASC),
    CONSTRAINT [FK_Systempropertyhousedepositfees_Propertyhouse] FOREIGN KEY ([Propertyhouseid]) REFERENCES [dbo].[Systempropertyhouses] ([Propertyhouseid])
);

