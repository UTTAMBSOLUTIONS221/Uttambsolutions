CREATE TABLE [dbo].[Systempropertyhousevacatingrequests] (
    [Vacatingrequestid]           INT            IDENTITY (1, 1) NOT NULL,
    [Systempropertyhousetenantid] BIGINT         NOT NULL,
    [Systempropertyhouseroomid]   INT            NOT NULL,
    [Plannedvacatingdate]         DATETIME       DEFAULT (getdate()) NOT NULL,
    [Expectedvacatingdate]        DATETIME       DEFAULT (getdate()) NOT NULL,
    [Vacatingreason]              VARCHAR (2000) NULL,
    [Vacatingstatus]              BIT            DEFAULT ((0)) NOT NULL,
    [Approvedby]                  BIGINT         NOT NULL,
    [Datecreated]                 DATETIME       DEFAULT (getdate()) NOT NULL,
    [Dateapproved]                DATETIME       DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Vacatingrequestid] ASC)
);

