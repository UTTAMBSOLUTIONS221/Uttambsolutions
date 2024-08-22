CREATE TABLE [dbo].[Initiatives] (
    [InitiativeId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [KeyResultId]  BIGINT         NULL,
    [Title]        NVARCHAR (255) NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [StartDate]    DATE           NULL,
    [EndDate]      DATE           NULL,
    [OwnerId]      BIGINT         NULL,
    [Status]       NVARCHAR (50)  NULL,
    [CreatedDate]  DATETIME       CONSTRAINT [DF__Initiativ__Creat__5BE2A6F2] DEFAULT (getdate()) NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF__Initiativ__Modif__5CD6CB2B] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK__Initiati__4EC4CDCBFEF6FFE9] PRIMARY KEY CLUSTERED ([InitiativeId] ASC),
    CONSTRAINT [FK__Initiativ__KeyRe__5AEE82B9] FOREIGN KEY ([KeyResultId]) REFERENCES [dbo].[KeyResults] ([KeyResultId])
);

