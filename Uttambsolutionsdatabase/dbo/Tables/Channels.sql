CREATE TABLE [dbo].[Channels] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [ChannelCode] INT          NOT NULL,
    [ChannelName] VARCHAR (25) NOT NULL,
    CONSTRAINT [pk_Channels] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [uk_Channels] UNIQUE NONCLUSTERED ([ChannelCode] ASC)
);

