CREATE TABLE [dbo].[Objectives] (
    [ObjectiveId]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (255) NOT NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [StartDate]      DATE           NULL,
    [EndDate]        DATE           NULL,
    [OwnerId]        BIGINT         NULL,
    [OrgDeptId]      BIGINT         NULL,
    [Status]         NVARCHAR (50)  NULL,
    [Isorganization] BIT            CONSTRAINT [DF__Objective__Isorg__5165187F] DEFAULT ((0)) NOT NULL,
    [CreatedDate]    DATETIME       CONSTRAINT [DF__Objective__Creat__52593CB8] DEFAULT (getdate()) NULL,
    [ModifiedDate]   DATETIME       CONSTRAINT [DF__Objective__Modif__534D60F1] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK__Objectiv__8C5633AD81615785] PRIMARY KEY CLUSTERED ([ObjectiveId] ASC)
);

