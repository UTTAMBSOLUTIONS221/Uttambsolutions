CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhouseroomfixturesdatabyhouseroomid]
@Houseroomid INT,
@Systempropertyhouseroomfixturesdata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		 SET @Systempropertyhouseroomfixturesdata= 
			(SELECT(SELECT ROOM.Systempropertyhouseroomid,ROOM.Systempropertyhouseid,
			  (CASE 
                    WHEN EXISTS (SELECT 1 FROM Systempropertyhousechecklists WHERE Propertyhouseroomid = ROOM.Systempropertyhouseroomid)
                    THEN 
                    (SELECT ISNULL(CQL.Propertychecklistid, 0) AS Propertychecklistid,
                            ISNULL(CQL.Propertyhouseroomid, 0) AS Propertyhouseroomid,
                            ISNULL(CQL.Fixturestatusid, 0) AS Fixturestatusid,
                            ISNULL(CQL.Fixtureunits, 0) AS Fixtureunits,
                            ISNULL(STS.Fixturestatus, 'Not Set') AS Fixturestatus,
                            ISNULL(CQL.Createdby, 0) AS Createdby,
                            ISNULL(CQL.Datecreated, GETDATE()) AS Datecreated,
                            FIX.Fixtureid,
                            FIX.Fixturetype,
                            FIX.Descriptions,
                            FIX.Category
                    FROM Systemfixtures FIX
                    LEFT JOIN Systempropertyhousechecklists CQL ON CQL.Fixtureid = FIX.Fixtureid
                    LEFT JOIN Systemfixturestatus STS ON CQL.Fixturestatusid = STS.Fixturestatusid
                    WHERE CQL.Propertyhouseroomid = ROOM.Systempropertyhouseroomid
                    FOR JSON PATH
                    )ELSE 
                    (SELECT 0 AS Propertychecklistid,
                            0 AS Fixturestatusid,
                            0 AS Fixtureunits,
                            '' AS Fixturestatus,
                            FIX.Fixtureid,
                            FIX.Fixturetype,
                            FIX.Descriptions,
                            FIX.Category
                    FROM Systemfixtures FIX
                    FOR JSON PATH
                    )
                END) AS Roomfixtures
			  FROM Systempropertyhouserooms ROOM
			  WHERE ROOM.Systempropertyhouseroomid =@Houseroomid
			  FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			  )AS Data
			 FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			);

	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systempropertyhouseroomfixturesdata AS Systempropertyhouseroomfixturesdata;

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