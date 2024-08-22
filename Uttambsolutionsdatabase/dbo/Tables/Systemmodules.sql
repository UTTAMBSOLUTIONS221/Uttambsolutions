CREATE TABLE [dbo].[Systemmodules] (
    [Moduleid]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [Modulename]        VARCHAR (100) NOT NULL,
    [Moduledescription] VARCHAR (300) NOT NULL,
    [Slug]              VARCHAR (100) NOT NULL,
    [Moduleimagepath]   VARCHAR (200) NULL,
    CONSTRAINT [PK__Systemmo__2B774B7FB1414ED6] PRIMARY KEY CLUSTERED ([Moduleid] ASC),
    CONSTRAINT [UQ__Systemmo__BC7B5FB6B2C06577] UNIQUE NONCLUSTERED ([Slug] ASC)
);

