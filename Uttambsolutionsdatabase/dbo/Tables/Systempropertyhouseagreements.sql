CREATE TABLE [dbo].[Systempropertyhouseagreements] (
    [Agreementid]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [Propertyhouseid]   BIGINT        NOT NULL,
    [Ownerortenantid]   BIGINT        NOT NULL,
    [Agreementname]     VARCHAR (100) NOT NULL,
    [Ownerortenant]     VARCHAR (20)  NOT NULL,
    [Signatureimageurl] VARCHAR (200) NOT NULL,
    [Datecreated]       DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Agreementid] ASC)
);

