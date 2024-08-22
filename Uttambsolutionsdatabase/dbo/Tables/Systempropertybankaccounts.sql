CREATE TABLE [dbo].[Systempropertybankaccounts] (
    [Systempropertybankaccountid]   BIGINT       IDENTITY (1, 1) NOT NULL,
    [Propertyhouseid]               BIGINT       NOT NULL,
    [Systembankid]                  BIGINT       NOT NULL,
    [Systempropertybankaccount]     VARCHAR (20) NOT NULL,
    [Systempropertyhousebankwehave] BIT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Systempropertybankaccountid] ASC)
);

