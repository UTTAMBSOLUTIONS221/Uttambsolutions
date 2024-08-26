CREATE TABLE [dbo].[Systemfixtures] (
    [Fixtureid]    INT            IDENTITY (1, 1) NOT NULL,
    [Fixturetype]  NVARCHAR (50)  NULL,
    [Descriptions] NVARCHAR (255) NULL,
    [Category]     NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([Fixtureid] ASC)
);

