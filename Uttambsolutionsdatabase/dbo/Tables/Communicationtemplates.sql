CREATE TABLE [dbo].[Communicationtemplates] (
    [Templateid]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [Templatename]    VARCHAR (100) NOT NULL,
    [Templatesubject] VARCHAR (100) NOT NULL,
    [Templatebody]    VARCHAR (MAX) NOT NULL,
    [Isactive]        BIT           CONSTRAINT [DF__Communica__Isact__25869641] DEFAULT ((1)) NOT NULL,
    [Isdeleted]       BIT           CONSTRAINT [DF__Communica__Isdel__267ABA7A] DEFAULT ((0)) NOT NULL,
    [Isemailsms]      BIT           NOT NULL,
    CONSTRAINT [PK__Communic__F865987F80516B7C] PRIMARY KEY CLUSTERED ([Templateid] ASC),
    CONSTRAINT [UQ__Communic__F626049CA9B6DBE2] UNIQUE NONCLUSTERED ([Templatename] ASC)
);

