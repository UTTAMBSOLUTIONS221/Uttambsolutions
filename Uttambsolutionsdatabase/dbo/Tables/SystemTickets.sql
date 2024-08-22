﻿CREATE TABLE [dbo].[SystemTickets] (
    [TicketId]             BIGINT   IDENTITY (1, 1) NOT NULL,
    [FinanceTransactionId] BIGINT   NOT NULL,
    [HouseRoomId]          BIGINT   NOT NULL,
    [AccountId]            BIGINT   NOT NULL,
    [Createdby]            BIGINT   NULL,
    [ActualDate]           DATETIME NOT NULL,
    [DateCreated]          DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([TicketId] ASC),
    FOREIGN KEY ([FinanceTransactionId]) REFERENCES [dbo].[FinanceTransactions] ([FinanceTransactionId]),
    FOREIGN KEY ([FinanceTransactionId]) REFERENCES [dbo].[FinanceTransactions] ([FinanceTransactionId])
);

