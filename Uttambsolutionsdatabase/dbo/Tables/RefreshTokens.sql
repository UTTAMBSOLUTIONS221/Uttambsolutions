CREATE TABLE [dbo].[RefreshTokens] (
    [Refreshtokenid] INT            IDENTITY (1, 1) NOT NULL,
    [Token]          NVARCHAR (MAX) NOT NULL,
    [Expirydate]     DATETIME       DEFAULT (getdate()) NOT NULL,
    [Userid]         BIGINT         NULL,
    PRIMARY KEY CLUSTERED ([Refreshtokenid] ASC),
    FOREIGN KEY ([Userid]) REFERENCES [dbo].[Systemstaffs] ([Userid])
);

