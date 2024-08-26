CREATE TABLE [dbo].[Systempropertyhouseimages] (
    [Propertyimageid]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [Propertyhouseid]     BIGINT        NOT NULL,
    [Houseorroom]         VARCHAR (20)  NOT NULL,
    [Houseorroomimageurl] VARCHAR (200) NOT NULL,
    [Createdby]           BIGINT        NOT NULL,
    [Datecreated]         DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Propertyimageid] ASC)
);

