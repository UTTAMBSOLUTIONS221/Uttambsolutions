CREATE TABLE [dbo].[Systemservices] (
    [Serviceid]       INT             IDENTITY (1, 1) NOT NULL,
    [Servicename]     VARCHAR (80)    NOT NULL,
    [Subscriptionfee] DECIMAL (10, 2) NOT NULL,
    [Isvisible]       BIT             DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Serviceid] ASC)
);

