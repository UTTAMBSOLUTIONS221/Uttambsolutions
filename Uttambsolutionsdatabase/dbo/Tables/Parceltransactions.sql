CREATE TABLE [dbo].[Parceltransactions] (
    [Transactionid]   INT             IDENTITY (1, 1) NOT NULL,
    [Parcelid]        INT             NULL,
    [Amount]          DECIMAL (10, 2) NULL,
    [Paymentmethodid] INT             NULL,
    [Transactiondate] DATETIME        DEFAULT (getdate()) NULL,
    [Parcelstatusid]  INT             NULL,
    PRIMARY KEY CLUSTERED ([Transactionid] ASC),
    FOREIGN KEY ([Parcelid]) REFERENCES [dbo].[Parcels] ([Parcelid]),
    FOREIGN KEY ([Parcelstatusid]) REFERENCES [dbo].[Parcelstatus] ([Parcelstatusid]),
    FOREIGN KEY ([Paymentmethodid]) REFERENCES [dbo].[Paymentmethods] ([Paymentmethodid])
);

