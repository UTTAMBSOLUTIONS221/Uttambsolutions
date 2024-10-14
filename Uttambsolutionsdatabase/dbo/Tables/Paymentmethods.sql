CREATE TABLE [dbo].[Paymentmethods] (
    [Paymentmethodid]   INT          IDENTITY (1, 1) NOT NULL,
    [Paymentmethodname] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Paymentmethodid] ASC)
);

