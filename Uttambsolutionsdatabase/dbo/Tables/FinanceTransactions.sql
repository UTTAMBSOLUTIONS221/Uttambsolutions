﻿CREATE TABLE [dbo].[FinanceTransactions] (
    [FinanceTransactionId]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [Tenantid]                    BIGINT        NOT NULL,
    [TransactionCode]             VARCHAR (100) NOT NULL,
    [FinanceTransactionTypeId]    BIGINT        NOT NULL,
    [FinanceTransactionSubTypeId] BIGINT        NOT NULL,
    [ParentId]                    BIGINT        DEFAULT ((0)) NOT NULL,
    [Saledescription]             VARCHAR (100) NULL,
    [IsOnlineSale]                BIT           DEFAULT ((0)) NOT NULL,
    [Isreversed]                  BIT           DEFAULT ((0)) NOT NULL,
    [IsDeleted]                   BIT           DEFAULT ((0)) NOT NULL,
    [Createdby]                   BIGINT        NOT NULL,
    [ActualDate]                  DATETIME      NOT NULL,
    [DateCreated]                 DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([FinanceTransactionId] ASC),
    FOREIGN KEY ([FinanceTransactionSubTypeId]) REFERENCES [dbo].[FinanceTransactionSubTypes] ([FinanceTransactionSubTypeId]),
    FOREIGN KEY ([FinanceTransactionSubTypeId]) REFERENCES [dbo].[FinanceTransactionSubTypes] ([FinanceTransactionSubTypeId]),
    FOREIGN KEY ([FinanceTransactionSubTypeId]) REFERENCES [dbo].[FinanceTransactionSubTypes] ([FinanceTransactionSubTypeId]),
    FOREIGN KEY ([FinanceTransactionSubTypeId]) REFERENCES [dbo].[FinanceTransactionSubTypes] ([FinanceTransactionSubTypeId]),
    FOREIGN KEY ([FinanceTransactionTypeId]) REFERENCES [dbo].[FinanceTransactionTypes] ([FinanceTransactionTypeId]),
    FOREIGN KEY ([FinanceTransactionTypeId]) REFERENCES [dbo].[FinanceTransactionTypes] ([FinanceTransactionTypeId]),
    FOREIGN KEY ([FinanceTransactionTypeId]) REFERENCES [dbo].[FinanceTransactionTypes] ([FinanceTransactionTypeId]),
    FOREIGN KEY ([FinanceTransactionTypeId]) REFERENCES [dbo].[FinanceTransactionTypes] ([FinanceTransactionTypeId])
);

