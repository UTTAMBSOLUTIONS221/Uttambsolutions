CREATE TABLE [dbo].[Jobtypes] (
    [Jobtypeid]   INT            IDENTITY (1, 1) NOT NULL,
    [Jobtypename] NVARCHAR (100) NOT NULL,
    [DateCreated] DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Jobtypeid] ASC)
);

