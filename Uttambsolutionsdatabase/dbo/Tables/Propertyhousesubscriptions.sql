CREATE TABLE [dbo].[Propertyhousesubscriptions] (
    [Propertyhousesubscriptionid]     INT             IDENTITY (1, 1) NOT NULL,
    [Propertyownerid]                 BIGINT          NOT NULL,
    [Propertyhouseid]                 BIGINT          NOT NULL,
    [Propertyhousesubscriptionamount] DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Propertyhousesubscriptionid] ASC)
);

