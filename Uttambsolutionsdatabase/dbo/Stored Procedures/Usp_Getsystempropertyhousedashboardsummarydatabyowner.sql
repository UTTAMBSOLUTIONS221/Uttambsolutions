CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhousedashboardsummarydatabyowner]
@Ownerid INT,
@Posterid INT,
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
		       SELECT(SELECT SUM(Systempropertyhousesize.Systempropertyhousesizeunits) AS Propertyhouseunits, 0 AS Systempropertyoccupiedroom, 0 AS Systempropertyvacantroom,
				0 Rentarrears,0 AS Uncollectedpayments,SUM(Systempropertyhouseroommeter.Movedmeter)  AS Consumedmeters,
				(SELECT Systempropertyhouse.Propertyhousename,SUM(Systempropertyhousesize.Systempropertyhousesizeunits) AS Propertyhouseunits, 0 AS Systempropertyoccupiedroom, 0 AS Systempropertyvacantroom,
				0 Rentarrears,0 AS Uncollectedpayments,SUM(Systempropertyhouseroommeter.Movedmeter)  AS Consumedmeters
				FROM Systempropertyhouses Systempropertyhouse
				INNER JOIN Systempropertyhouserooms Systempropertyhouseroom ON Systempropertyhouseroom.Systempropertyhouseid =Systempropertyhouse.Propertyhouseid
				INNER JOIN Systempropertyhousesizes Systempropertyhousesize ON Systempropertyhousesize.Propertyhouseid=Systempropertyhouse.Propertyhouseid
				INNER JOIN Systempropertyhouseroommeters Systempropertyhouseroommeter  ON Systempropertyhouseroom.Systempropertyhouseroomid=Systempropertyhouseroommeter.Systempropertyhouseroomid
				GROUP BY Systempropertyhouse.Propertyhousename
				FOR JSON PATH) AS Propertybysummary
				FROM Systempropertyhouses Systempropertyhouse
				INNER JOIN Systempropertyhouserooms Systempropertyhouseroom ON Systempropertyhouseroom.Systempropertyhouseid =Systempropertyhouse.Propertyhouseid
				INNER JOIN Systempropertyhousesizes Systempropertyhousesize ON Systempropertyhousesize.Propertyhouseid=Systempropertyhouse.Propertyhouseid
				INNER JOIN Systempropertyhouseroommeters Systempropertyhouseroommeter  ON Systempropertyhouseroom.Systempropertyhouseroomid=Systempropertyhouseroommeter.Systempropertyhouseroomid
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
