CREATE TABLE [dbo].[KeyResults] (
    [KeyResultId]  BIGINT          IDENTITY (1, 1) NOT NULL,
    [ObjectiveId]  BIGINT          NULL,
    [Description]  NVARCHAR (255)  NOT NULL,
    [TargetValue]  DECIMAL (18, 2) NULL,
    [CurrentValue] DECIMAL (18, 2) NULL,
    [StartDate]    DATE            NULL,
    [EndDate]      DATE            NULL,
    [Status]       NVARCHAR (50)   NULL,
    [OwnerId]      BIGINT          NULL,
    [CreatedDate]  DATETIME        CONSTRAINT [DF__KeyResult__Creat__571DF1D5] DEFAULT (getdate()) NULL,
    [ModifiedDate] DATETIME        CONSTRAINT [DF__KeyResult__Modif__5812160E] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK__KeyResul__F86A90BDAE991EEF] PRIMARY KEY CLUSTERED ([KeyResultId] ASC),
    CONSTRAINT [FK__KeyResult__Objec__5629CD9C] FOREIGN KEY ([ObjectiveId]) REFERENCES [dbo].[Objectives] ([ObjectiveId])
);

