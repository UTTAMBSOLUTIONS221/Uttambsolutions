CREATE TABLE [dbo].[SettingItems] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [ItemCode] INT           NOT NULL,
    [ItemName] VARCHAR (50)  NOT NULL,
    [Descr]    VARCHAR (500) NULL,
    [Categ]    INT           NOT NULL,
    [SubCateg] INT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [pk_SettingItems] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [uk_SettingItems_ItemCode] UNIQUE NONCLUSTERED ([ItemCode] ASC),
    CONSTRAINT [uk_SettingItems_ItemName] UNIQUE NONCLUSTERED ([ItemName] ASC)
);

