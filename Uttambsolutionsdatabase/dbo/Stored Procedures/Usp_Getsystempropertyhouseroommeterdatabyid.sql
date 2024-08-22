CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhouseroommeterdatabyid]
@Houseroomid BIGINT,
@Systempropertyhousemetedata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
	       IF EXISTS (SELECT 1 FROM Systempropertyhouseroommeters WHERE Systempropertyhouseroomid = @Houseroomid)
		BEGIN
				SET @Systempropertyhousemetedata =
					(SELECT 
						1 AS Hasprevious,
						Systempropertyhouseroommeter.Systempropertyhousemeterid,
						Systempropertyhouseroommeter.Systempropertyhouseroomid,
						Systempropertyhouseroommeter.Systempropertyhouseroommeternumber,
						Systempropertyhouseroommeter.Openingmeter,
						Systempropertyhouseroommeter.Movedmeter,
						Systempropertyhouseroommeter.Closingmeter,
						Systempropertyhouseroommeter.Createdby,
						Systempropertyhouseroommeter.Datecreated,
						(
							SELECT 
								Systempropertyhouseroommeterhist.Systempropertyhousemeterid,
								Systempropertyhouseroommeterhist.Systempropertyhouseroomid,
								Systempropertyhouseroommeterhist.Systempropertyhouseroommeternumber,
								Systempropertyhouseroommeterhist.Openingmeter,
								Systempropertyhouseroommeterhist.Movedmeter,
								Systempropertyhouseroommeterhist.Closingmeter,
								Systempropertyhouseroommeterhist.Createdby,
								Systempropertyhouseroommeterhist.Datecreated
							FROM Systempropertyhouseroommeters Systempropertyhouseroommeterhist
							WHERE Systempropertyhouseroommeter.Systempropertyhousemeterid = Systempropertyhouseroommeterhist.Systempropertyhousemeterid
							FOR JSON PATH
						) AS Data
					FROM Systempropertyhouseroommeters Systempropertyhouseroommeter
					WHERE Systempropertyhouseroommeter.Systempropertyhouseroomid = @Houseroomid
					FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER);
			END
			ELSE
			BEGIN
				SET @Systempropertyhousemetedata =
					(SELECT 
						0 AS Hasprevious,
						0 AS Systempropertyhousemeterid,
						@Houseroomid AS Systempropertyhouseroomid,
						'Meter123' AS Systempropertyhouseroommeternumber,
						0.00 AS Openingmeter,
						0.00 AS Movedmeter,
						0.00 AS Closingmeter,
						0 AS Createdby,
						GETDATE() AS Datecreated,
						NULL AS Data
					FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER);
			END
	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systempropertyhousemetedata AS Systempropertyhousemetedata;

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
