CREATE TABLE [dbo].[Subscriptionmultiplier] (
    [Subscriptionmultiplierid]    INT             IDENTITY (1, 1) NOT NULL,
    [Subscriptionmultiplier]      VARCHAR (70)    DEFAULT ('Rental Subscription') NOT NULL,
    [Subscriptionmultipliervalue] DECIMAL (10, 2) DEFAULT ((0.1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Subscriptionmultiplierid] ASC)
);

