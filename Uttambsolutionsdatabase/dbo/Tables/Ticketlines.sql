CREATE TABLE [dbo].[Ticketlines] (
    [TicketlineId]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [TicketId]                        BIGINT          NOT NULL,
    [Systempropertyhousedepositfeeid] BIGINT          NOT NULL,
    [Units]                           DECIMAL (18, 2) NOT NULL,
    [Price]                           DECIMAL (18, 2) NOT NULL,
    [Discount]                        DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [Createdby]                       BIGINT          NOT NULL,
    [DateCreated]                     DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([TicketlineId] ASC),
    FOREIGN KEY ([TicketId]) REFERENCES [dbo].[SystemTickets] ([TicketId]),
    FOREIGN KEY ([TicketId]) REFERENCES [dbo].[SystemTickets] ([TicketId])
);

