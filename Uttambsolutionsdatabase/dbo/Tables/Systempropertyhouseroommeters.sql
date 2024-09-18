CREATE TABLE [dbo].[Systempropertyhouseroommeters] (
    [Systempropertyhousemeterid]         INT             IDENTITY (1, 1) NOT NULL,
    [Systempropertyhouseroomid]          INT             NOT NULL,
    [Systempropertyhouseroommeternumber] VARCHAR (100)   NOT NULL,
    [Periodid]                           INT             NOT NULL,
    [Meterreadingstatus]                 VARCHAR (20)    NOT NULL,
    [Openingmeter]                       DECIMAL (18, 2) NOT NULL,
    [Movedmeter]                         DECIMAL (18, 2) NOT NULL,
    [Closingmeter]                       DECIMAL (18, 2) NOT NULL,
    [Consumedamount]                     DECIMAL (18, 2) NOT NULL,
    [Createdby]                          INT             NOT NULL,
    [Datecreated]                        DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([Systempropertyhousemeterid] ASC)
);




GO
