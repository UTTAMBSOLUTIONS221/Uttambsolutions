CREATE TABLE [dbo].[Jobindustries] (
    [Jobindustryid]   INT            IDENTITY (1, 1) NOT NULL,
    [Jobindustryname] NVARCHAR (100) NOT NULL,
    [DateCreated]     DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Jobindustryid] ASC)
);

