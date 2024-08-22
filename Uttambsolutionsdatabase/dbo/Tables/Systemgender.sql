CREATE TABLE [dbo].[Systemgender] (
    [Genderid]   BIGINT       IDENTITY (1, 1) NOT NULL,
    [Gendername] VARCHAR (40) NOT NULL,
    PRIMARY KEY CLUSTERED ([Genderid] ASC)
);

