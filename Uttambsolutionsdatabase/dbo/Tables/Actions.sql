CREATE TABLE [dbo].[Actions] (
    [ActionId]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [InitiativeId] BIGINT         NULL,
    [Description]  NVARCHAR (255) NOT NULL,
    [StartDate]    DATE           NULL,
    [EndDate]      DATE           NULL,
    [OwnerId]      BIGINT         NULL,
    [Status]       NVARCHAR (50)  NULL,
    [CreatedDate]  DATETIME       CONSTRAINT [DF__Actions__Created__60A75C0F] DEFAULT (getdate()) NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF__Actions__Modifie__619B8048] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK__Actions__FFE3F4D932690382] PRIMARY KEY CLUSTERED ([ActionId] ASC),
    CONSTRAINT [FK__Actions__Initiat__5FB337D6] FOREIGN KEY ([InitiativeId]) REFERENCES [dbo].[Initiatives] ([InitiativeId])
);

