CREATE TABLE [dbo].[Monthlyrentinvoices] (
    [Invoiceid]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [Invoiceno]                 VARCHAR (10)    NOT NULL,
    [Propertyhouseroomid]       BIGINT          NOT NULL,
    [Propertyhouseroomtenantid] BIGINT          NOT NULL,
    [Financetransactionid]      BIGINT          NOT NULL,
    [Datecreated]               DATETIME        NOT NULL,
    [Duedate]                   DATETIME        NOT NULL,
    [Amount]                    DECIMAL (18, 2) NOT NULL,
    [Discount]                  DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [Ispaid]                    BIT             DEFAULT ((0)) NOT NULL,
    [Paidamount]                DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [Balance]                   DECIMAL (18, 2) NOT NULL,
    [Issent]                    BIT             DEFAULT ((0)) NOT NULL,
    [Paidstatus]                VARCHAR (40)    DEFAULT ('NOT PAID') NOT NULL,
    PRIMARY KEY CLUSTERED ([Invoiceid] ASC)
);

