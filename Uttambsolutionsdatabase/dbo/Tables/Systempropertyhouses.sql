CREATE TABLE [dbo].[Systempropertyhouses] (
    [Propertyhouseid]         BIGINT          IDENTITY (1, 1) NOT NULL,
    [Housecode]               VARCHAR (10)    NOT NULL,
    [Isagency]                BIT             DEFAULT ((0)) NOT NULL,
    [Propertyhouseowner]      BIGINT          NOT NULL,
    [Propertyhouseposter]     BIGINT          NOT NULL,
    [Propertyhousename]       VARCHAR (200)   NOT NULL,
    [Countyid]                INT             NOT NULL,
    [Subcountyid]             INT             NOT NULL,
    [Subcountywardid]         INT             NOT NULL,
    [Streetorlandmark]        VARCHAR (200)   NOT NULL,
    [Contactdetails]          VARCHAR (200)   NOT NULL,
    [Rentingterms]            VARCHAR (20)    DEFAULT ('Month to Month') NOT NULL,
    [Enddate]                 DATETIME        DEFAULT (getdate()) NOT NULL,
    [Hashousedeposit]         BIT             DEFAULT ((0)) NOT NULL,
    [Rentdepositmonth]        INT             NOT NULL,
    [Rentdepositreturndays]   INT             DEFAULT ((0)) NOT NULL,
    [Rentutilityinclusive]    BIT             DEFAULT ((0)) NOT NULL,
    [Propertyhousestatus]     INT             DEFAULT ((0)) NOT NULL,
    [Hasagent]                BIT             DEFAULT ((0)) NOT NULL,
    [Allowpets]               BIT             DEFAULT ((0)) NOT NULL,
    [Numberofpets]            INT             DEFAULT ((0)) NOT NULL,
    [Petdeposit]              DECIMAL (10, 2) DEFAULT ((0)) NOT NULL,
    [Petparticulars]          VARCHAR (200)   DEFAULT ('Not Applicable') NULL,
    [Watertypeid]             INT             NOT NULL,
    [Hashousewatermeter]      BIT             NOT NULL,
    [Waterunitprice]          DECIMAL (10, 2) NOT NULL,
    [Rentdueday]              INT             NOT NULL,
    [Vacantnoticeperiod]      INT             NOT NULL,
    [Monthlycollection]       DECIMAL (18, 2) NOT NULL,
    [Actualmonthlycollection] DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [Extra]                   VARCHAR (200)   NULL,
    [Extra1]                  VARCHAR (200)   NULL,
    [Extra2]                  VARCHAR (200)   NULL,
    [Extra3]                  VARCHAR (200)   NULL,
    [Extra4]                  VARCHAR (200)   NULL,
    [Extra5]                  VARCHAR (200)   NULL,
    [Extra6]                  VARCHAR (200)   NULL,
    [Extra7]                  VARCHAR (200)   NULL,
    [Extra8]                  VARCHAR (200)   NULL,
    [Extra9]                  VARCHAR (200)   NULL,
    [Extra10]                 VARCHAR (200)   NULL,
    [Createdby]               BIGINT          NOT NULL,
    [Modifiedby]              BIGINT          NOT NULL,
    [Datecreated]             DATETIME        NOT NULL,
    [Datemodified]            DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([Propertyhouseid] ASC)
);






GO

CREATE TRIGGER [dbo].[trg_PropertyHouseMonthlySubscriptions]
ON [dbo].[Systempropertyhouses]
AFTER INSERT, UPDATE
AS
BEGIN
    -- Declare a variable to hold the multiplier value
    DECLARE @Multiplier DECIMAL(18,2);

    SELECT @Multiplier = Subscriptionmultipliervalue FROM Subscriptionmultiplier

    -- Insert operation
	MERGE INTO Propertyhousesubscriptions AS target
	USING inserted AS source
	ON target.Propertyownerid = source.Propertyhouseowner AND target.Propertyhouseid = source.Propertyhouseid
	WHEN NOT MATCHED BY TARGET THEN
	INSERT (Propertyownerid, Propertyhouseid, Propertyhousesubscriptionamount)
	VALUES (source.Propertyhouseowner,source.Propertyhouseid,source.Monthlycollection * CASE WHEN source.Monthlycollection < 100000 THEN 0.02 ELSE @Multiplier END)
	WHEN MATCHED THEN
	UPDATE SET target.Propertyhousesubscriptionamount = source.Monthlycollection * CASE WHEN source.Monthlycollection < 100000 THEN 0.02 ELSE @Multiplier END;

END;