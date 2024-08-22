CREATE TABLE [dbo].[Systemaccountverifaction] (
    [Verificationid]        INT          IDENTITY (1, 1) NOT NULL,
    [Verificationname]      VARCHAR (40) NOT NULL,
    [Verificationtype]      VARCHAR (20) NOT NULL,
    [Verificationshortcode] INT          NOT NULL,
    [Accountnumber]         VARCHAR (40) NOT NULL,
    [Isactive]              BIT          DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Verificationid] ASC)
);

