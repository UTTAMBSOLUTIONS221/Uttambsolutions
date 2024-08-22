CREATE TABLE [dbo].[Systempropertyhousesizes] (
    [Systempropertyhousesizeid]     BIGINT IDENTITY (1, 1) NOT NULL,
    [Propertyhouseid]               BIGINT NOT NULL,
    [Systemhousesizeid]             BIGINT NOT NULL,
    [Systempropertyhousesizeunits]  INT    NOT NULL,
    [Systempropertyhousesizewehave] BIT    DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Systempropertyhousesizeid] ASC),
    CONSTRAINT [FK_Systempropertyhousesizes_Propertyhouse] FOREIGN KEY ([Propertyhouseid]) REFERENCES [dbo].[Systempropertyhouses] ([Propertyhouseid])
);


GO



CREATE TRIGGER [dbo].[trg_AfterInsertUpdate_Systempropertyhousesizes]
ON [dbo].[Systempropertyhousesizes]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Table variable to hold the merged results
    DECLARE @MergedData TABLE (
	    Systempropertyhouseid BIGINT,
        Systempropertyhousesizeid BIGINT,
        Systempropertyhousesizename VARCHAR(100),
        RowNumber INT
    );

    -- Insert the data into the table variable
    INSERT INTO @MergedData (Systempropertyhouseid,Systempropertyhousesizeid, Systempropertyhousesizename, RowNumber)
    SELECT  i.Propertyhouseid, i.Systempropertyhousesizeid,
           CONCAT(s.Systemhousesizename, ' Room ', rn.RowNumber) AS Systempropertyhousesizename,
           rn.RowNumber
    FROM inserted i
    INNER JOIN Systemhousesizes s ON i.Systemhousesizeid = s.Systemhousesizeid
    CROSS APPLY (
        SELECT TOP (i.Systempropertyhousesizeunits) 
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNumber
        FROM master..spt_values -- This is a system table with a lot of rows. Adjust as needed.
    ) AS rn
    WHERE i.Systempropertyhousesizeunits > 0;

    -- Merge into Systempropertyhouserooms
    MERGE INTO Systempropertyhouserooms AS target
    USING @MergedData AS source
    ON target.Systempropertyhousesizeid = source.Systempropertyhousesizeid
    AND target.Systempropertyhousesizename = source.Systempropertyhousesizename
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (Systempropertyhouseid,Systempropertyhousesizeid,Systempropertyhousesizename,Systempropertyhousesizerent,Systempropertyhousesizedeposit,Isvacant,Isunderrenovation,Isshop,Isgroundfloor,Hasbalcony,Forcaretaker,Kitchentypeid)
        VALUES (source.Systempropertyhouseid,source.Systempropertyhousesizeid, source.Systempropertyhousesizename,0,0, 1, 0, 0, 0,0,0,1);

    SET NOCOUNT OFF;
END;


ALTER TABLE [dbo].[Systempropertyhousesizes] ENABLE TRIGGER [trg_AfterInsertUpdate_Systempropertyhousesizes]
