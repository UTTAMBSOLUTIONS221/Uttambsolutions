CREATE TABLE [dbo].[Monthlyrentinvoiceitems] (
    [Invoiceitemid]                   BIGINT          IDENTITY (1, 1) NOT NULL,
    [Invoiceid]                       BIGINT          NOT NULL,
    [Systempropertyhousedepositfeeid] BIGINT          NOT NULL,
    [Units]                           DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [Price]                           DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [Discount]                        DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Invoiceitemid] ASC)
);

