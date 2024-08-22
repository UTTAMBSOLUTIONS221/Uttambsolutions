CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhouseroomdatabyid]
    @Houseroomid BIGINT,
    @Systempropertyhouseroomdata VARCHAR(MAX) OUTPUT
AS
BEGIN
    DECLARE 
        @RespStat INT = 0,
        @RespMsg VARCHAR(150) = 'Ok',
        @Systempropertyhousemetedata VARCHAR(MAX);

    BEGIN TRY
        BEGIN TRANSACTION;

        SET @Systempropertyhouseroomdata = (
		  CASE 
			WHEN EXISTS (SELECT 1 FROM Systempropertyhouseroommeters WHERE Systempropertyhouseroomid = @Houseroomid)
			THEN (
				SELECT (SELECT TOP 1 1 AS Hasprevious,
					Systempropertyhouseroom.Systempropertyhouseroomid,
					Systempropertyhouseroom.Systempropertyhouseid,
					Systempropertyhousesize.Systemhousesizeid AS Systempropertyhousesizeid,
					Systempropertyhouseroom.Systempropertyhousesizename,
					Systempropertyhouseroom.Systempropertyhousesizerent,
					Systempropertyhouseroom.Systempropertyhousesizedeposit,
					Systempropertyhouseroom.Isvacant,
					Systempropertyhouseroom.Isunderrenovation,
					Systempropertyhouseroom.Isshop,
					Systempropertyhouseroom.Isgroundfloor,
					Systempropertyhouseroom.Hasbalcony,
					Systempropertyhouseroom.Forcaretaker,
					Systempropertyhouseroom.Kitchentypeid,
					Systempropertyhouseroommeter.Systempropertyhousemeterid,
					Systempropertyhouseroommeter.Systempropertyhouseroommeternumber,
					ISNULL((SELECT TOP 1 Closingmeter FROM Systempropertyhouseroommeters WHERE Systempropertyhouseroomid = Systempropertyhouseroom.Systempropertyhouseroomid ORDER BY Datecreated DESC), 0.00) AS Openingmeter,
					0 AS Movedmeter,
					0 AS Closingmeter,
					Systempropertyhouse.Waterunitprice,
					Systempropertyhouse.Hashousewatermeter,
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
							Systempropertyhouseroommeterhist.Consumedamount,
							Systempropertyhouseroommeterhist.Createdby,
							Systempropertyhouseroommeterhist.Datecreated
						FROM Systempropertyhouseroommeters Systempropertyhouseroommeterhist
						WHERE Systempropertyhouseroom.Systempropertyhouseroomid=Systempropertyhouseroommeterhist.Systempropertyhouseroomid
						FOR JSON PATH
					) AS Meterhistorydata,
					(
						SELECT 
							Systempropertyhouseroommeterhist.Systempropertyhousemeterid,
							Systempropertyhouseroommeterhist.Systempropertyhouseroomid,
							Systempropertyhouseroommeterhist.Systempropertyhouseroommeternumber,
							Systempropertyhouseroommeterhist.Openingmeter,
							Systempropertyhouseroommeterhist.Movedmeter,
							Systempropertyhouseroommeterhist.Closingmeter,
							Systempropertyhouseroommeterhist.Consumedamount,
							Systempropertyhouseroommeterhist.Createdby,
							Systempropertyhouseroommeterhist.Datecreated
						FROM Systempropertyhouseroommeters Systempropertyhouseroommeterhist
						WHERE Systempropertyhouseroom.Systempropertyhouseroomid=Systempropertyhouseroommeterhist.Systempropertyhouseroomid
						FOR JSON PATH
					) AS Roomtenanthistorydata
				FROM Systempropertyhouserooms Systempropertyhouseroom
				INNER JOIN Systempropertyhouses Systempropertyhouse ON Systempropertyhouseroom.Systempropertyhouseid =Systempropertyhouse.Propertyhouseid
				INNER JOIN Systempropertyhousesizes Systempropertyhousesize ON Systempropertyhouseroom.Systempropertyhousesizeid=Systempropertyhousesize.Systempropertyhousesizeid
				INNER JOIN Systemhousesizes Systemhousesize ON Systempropertyhousesize.Systemhousesizeid=Systemhousesize.Systemhousesizeid
				LEFT JOIN Systempropertyhouseroommeters Systempropertyhouseroommeter ON Systempropertyhouseroom.Systempropertyhouseroomid=Systempropertyhouseroommeter.Systempropertyhouseroomid
                WHERE Systempropertyhouseroom.Systempropertyhouseroomid = @Houseroomid
                FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
                ) AS Data
				FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
				)
                ELSE (
                    SELECT(SELECT 
                        0 AS Hasprevious,
						Systempropertyhouseroom.Systempropertyhouseroomid,
						Systempropertyhouseroom.Systempropertyhouseid,
						Systempropertyhousesize.Systemhousesizeid AS Systempropertyhousesizeid,
						Systempropertyhouseroom.Systempropertyhousesizename,
						Systempropertyhouseroom.Systempropertyhousesizerent,
						Systempropertyhouseroom.Systempropertyhousesizedeposit,
						Systempropertyhouseroom.Isvacant,
						Systempropertyhouseroom.Isunderrenovation,
						Systempropertyhouseroom.Isshop,
						Systempropertyhouseroom.Isgroundfloor,
						Systempropertyhouseroom.Hasbalcony,
						Systempropertyhouseroom.Forcaretaker,
						Systempropertyhouseroom.Kitchentypeid,
                        0 AS Systempropertyhousemeterid,
                        'Meter123' AS Systempropertyhouseroommeternumber,
                        0.00 AS Openingmeter,
                        0.00 AS Movedmeter,
                        0.00 AS Closingmeter,
						Systempropertyhouse.Hashousewatermeter,
						Systempropertyhouse.Waterunitprice,
                        0 AS Createdby,
                        GETDATE() AS Datecreated,
                        NULL AS Meterhistorydata
				FROM Systempropertyhouserooms Systempropertyhouseroom
				INNER JOIN Systempropertyhouses Systempropertyhouse ON Systempropertyhouseroom.Systempropertyhouseid =Systempropertyhouse.Propertyhouseid
				INNER JOIN Systempropertyhousesizes Systempropertyhousesize ON Systempropertyhouseroom.Systempropertyhousesizeid=Systempropertyhousesize.Systempropertyhousesizeid
				INNER JOIN Systemhousesizes Systemhousesize ON Systempropertyhousesize.Systemhousesizeid=Systemhousesize.Systemhousesizeid
				LEFT JOIN Systempropertyhouseroommeters Systempropertyhouseroommeter ON Systempropertyhouseroom.Systempropertyhouseroomid=Systempropertyhouseroommeter.Systempropertyhouseroomid
                WHERE Systempropertyhouseroom.Systempropertyhouseroomid = @Houseroomid
				FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
              ) AS Data
			FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
			)
            END
        );

        SET @RespMsg = 'Ok.';
        SET @RespStat = 0;
        COMMIT TRANSACTION;

        SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage,@Systempropertyhouseroomdata AS Systempropertyhouseroomdata;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT 'Error ' + ERROR_MESSAGE();
        SELECT 2 AS RespStatus, '0 - Error(s) Occurred: ' + ERROR_MESSAGE() AS RespMessage;
    END CATCH

    RETURN; 
END
