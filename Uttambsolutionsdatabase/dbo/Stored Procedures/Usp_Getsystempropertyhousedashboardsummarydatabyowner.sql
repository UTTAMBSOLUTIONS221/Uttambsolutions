﻿CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhousedashboardsummarydatabyowner]
@Ownerid INT,
@Systempropertyhousedashboardsummarydata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		SET @Systempropertyhousedashboardsummarydata= 
		  (
		       SELECT(SELECT 
    ISNULL(COUNT(Systempropertyhouseroom.Systempropertyhouseid), 0) AS Propertyhouseunits,
      ISNULL(SUM(CASE WHEN Systempropertyhouseroom.Isvacant = 0 THEN 1 ELSE 0 END), 0) AS Systempropertyoccupiedroom,
       ISNULL(SUM(CASE WHEN Systempropertyhouseroom.Isvacant = 1 THEN 1 ELSE 0 END), 0) AS Systempropertyvacantroom,
    0 AS Rentarrears,
    0 AS Uncollectedpayments,
    ISNULL(SUM(Systempropertyhouseroommeter.Movedmeter), 0) AS Consumedmeters,
    (
        SELECT 
            Systempropertyhousedata.Propertyhousename,
            ISNULL(COUNT(Systempropertyhouseroom.Systempropertyhouseid), 0) AS Propertyhouseunits,
            ISNULL(SUM(CASE WHEN Systempropertyhouseroom.Isvacant = 0 THEN 1 ELSE 0 END), 0) AS Systempropertyoccupiedroom,
            ISNULL(SUM(CASE WHEN Systempropertyhouseroom.Isvacant = 1 THEN 1 ELSE 0 END), 0) AS Systempropertyvacantroom,
            0 AS Rentarrears,
            0 AS Uncollectedpayments,
            ISNULL(SUM(Systempropertyhouseroommeter.Movedmeter), 0) AS Consumedmeters
        FROM Systempropertyhouses Systempropertyhousedata
        INNER JOIN Systempropertyhouserooms Systempropertyhouseroom ON Systempropertyhousedata.Propertyhouseid = Systempropertyhouseroom.Systempropertyhouseid
        LEFT JOIN Systempropertyhouseroommeters Systempropertyhouseroommeter ON Systempropertyhouseroom.Systempropertyhouseroomid = Systempropertyhouseroommeter.Systempropertyhouseroomid
        WHERE Systempropertyhouse.Propertyhouseid = Systempropertyhousedata.Propertyhouseid
        GROUP BY Systempropertyhousedata.Propertyhousename
        FOR JSON PATH
    ) AS Propertybysummary
FROM Systempropertyhouses Systempropertyhouse
INNER JOIN Systempropertyhouserooms Systempropertyhouseroom ON Systempropertyhouse.Propertyhouseid = Systempropertyhouseroom.Systempropertyhouseid
LEFT JOIN Systempropertyhouseroommeters Systempropertyhouseroommeter ON Systempropertyhouseroom.Systempropertyhouseroomid = Systempropertyhouseroommeter.Systempropertyhouseroomid
WHERE Systempropertyhouse.Propertyhouseowner = @Ownerid AND Systempropertyhouse.Propertyhouseposter = @Ownerid
GROUP BY Systempropertyhouse.Propertyhouseid
				FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
				) AS Data
				FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
	     );


	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systempropertyhousedashboardsummarydata AS Systempropertyhousedashboardsummarydata;

		END TRY
		BEGIN CATCH
		ROLLBACK TRANSACTION
		PRINT ''
		PRINT 'Error ' + error_message();
		Select 2 as RespStatus, '0 - Error(s) Occurred' + error_message() as RespMessage
		END CATCH
		Select @RespStat as RespStatus, @RespMsg as RespMessage;
		RETURN; 
		END;
	END
END