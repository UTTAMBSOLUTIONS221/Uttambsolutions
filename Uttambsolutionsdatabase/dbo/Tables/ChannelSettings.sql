CREATE TABLE [dbo].[ChannelSettings] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [ChannelCode] INT           NOT NULL,
    [ItemCode]    INT           NOT NULL,
    [ItemValue]   VARCHAR (250) NOT NULL,
    CONSTRAINT [pk_ChannelSettings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [fk_ChannelSettings_ChannelCode] FOREIGN KEY ([ChannelCode]) REFERENCES [dbo].[Channels] ([ChannelCode]),
    CONSTRAINT [fk_ChannelSettings_ItemCode] FOREIGN KEY ([ItemCode]) REFERENCES [dbo].[SettingItems] ([ItemCode]),
    CONSTRAINT [uk_ChannelSettings_ChannelCode_ItemCode] UNIQUE NONCLUSTERED ([ChannelCode] ASC, [ItemCode] ASC)
);

