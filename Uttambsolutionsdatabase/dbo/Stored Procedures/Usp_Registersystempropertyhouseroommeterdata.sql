CREATE PROCEDURE [dbo].[Usp_Registersystempropertyhouseroommeterdata]
    @JsonObjectdata NVARCHAR(MAX)
AS
BEGIN
    DECLARE 
	        @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '',
			@Systempropertyhouseroomid INT,
			@Movedmeter DECIMAL(18, 2),
			@Consumedamount DECIMAL(18, 2);

    BEGIN TRY
        BEGIN TRANSACTION;
		SET @Systempropertyhouseroomid = JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroomid');
        SET @Movedmeter = JSON_VALUE(@JsonObjectdata, '$.Movedmeter');
        SET @Consumedamount = JSON_VALUE(@JsonObjectdata, '$.Consumedamount');

		MERGE INTO Systempropertyhouseroommeters AS target
		USING 
		(
			SELECT 
				JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroomid') AS Systempropertyhouseroomid,
				JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroommeternumber') AS Systempropertyhouseroommeternumber,
				(SELECT x.PeriodId FROM Systemperiods x WHERE x.Lastdateinperiod = (SELECT EOMONTH(GETDATE(), 0))) AS Periodid,
				'Closemeterreading' AS Meterreadingstatus,
				JSON_VALUE(@JsonObjectdata, '$.Openingmeter') AS Openingmeter,
				JSON_VALUE(@JsonObjectdata, '$.Movedmeter') AS Movedmeter,
				JSON_VALUE(@JsonObjectdata, '$.Closingmeter') AS Closingmeter,
				JSON_VALUE(@JsonObjectdata, '$.Consumedamount') AS Consumedamount,
				JSON_VALUE(@JsonObjectdata, '$.Createdby') AS Createdby,
				CAST(JSON_VALUE(@JsonObjectdata, '$.Datecreated') AS DATETIME2) AS Datecreated
		) AS source
		ON 
			target.Systempropertyhouseroomid = source.Systempropertyhouseroomid 
			AND target.Periodid = source.Periodid 
			AND target.Meterreadingstatus = 'Closemeterreading'
		WHEN MATCHED THEN
			UPDATE SET
				target.Openingmeter = source.Openingmeter,
				target.Movedmeter = source.Movedmeter,
				target.Closingmeter = source.Closingmeter,
				target.Consumedamount = source.Consumedamount,
				target.Createdby = source.Createdby,
				target.Datecreated = source.Datecreated
		WHEN NOT MATCHED THEN
			INSERT (Systempropertyhouseroomid, Systempropertyhouseroommeternumber, Periodid, Meterreadingstatus, Openingmeter, Movedmeter, Closingmeter, Consumedamount, Createdby, Datecreated)
			VALUES (source.Systempropertyhouseroomid, source.Systempropertyhouseroommeternumber, source.Periodid, source.Meterreadingstatus, source.Openingmeter, source.Movedmeter, source.Closingmeter, source.Consumedamount, source.Createdby, source.Datecreated);

        SET @RespMsg = 'Success.'
        SET @RespStat = 0; 
        
        COMMIT TRANSACTION;

        SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;
		 EXEC Usp_Generatemonthlysusequentinvoiceaftermeter @Systempropertyhouseroomid = @Systempropertyhouseroomid,@Consumedwaterunits = @Movedmeter,@Consumedwateramount = @Consumedamount;
	END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT ''
        PRINT 'Error ' + error_message();
        SELECT 2 AS RespStatus, '0 - Error(s) Occurred' + error_message() AS RespMessage;
    END CATCH

    SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;
END