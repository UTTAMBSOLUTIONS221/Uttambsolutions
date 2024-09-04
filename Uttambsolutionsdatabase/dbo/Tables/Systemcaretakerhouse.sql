CREATE TABLE [dbo].[Systemcaretakerhouse] (
    [Caretakerhouseid] BIGINT IDENTITY (1, 1) NOT NULL,
    [Caretakerid]      BIGINT NOT NULL,
    [Propertyhouseid]  BIGINT NOT NULL,
    PRIMARY KEY CLUSTERED ([Caretakerhouseid] ASC)
);

