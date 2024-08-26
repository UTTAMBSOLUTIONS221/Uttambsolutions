CREATE PROCEDURE [dbo].[Usp_Registersystempropertyhouseroomfixturedata]
@JsonObjectdata VARCHAR(MAX)
AS
BEGIN
   BEGIN
	DECLARE @RespStat int = 0,
			@RespMsg varchar(150) = '',
			@Propertyhouseid BIGINT = NULL;
		  
	BEGIN
		BEGIN TRY	
		--Validate

		BEGIN TRANSACTION;
		DECLARE @Systempropertyhousedata TABLE (Propertyhouseid BIGINT);

		MERGE INTO Systempropertyhousechecklists AS target
		USING (SELECT rf.Propertychecklistid,sphf.Systempropertyhouseroomid,rf.Fixtureid,rf.Fixtureunits,rf.Fixturestatusid,sphf.Createdby,sphf.Datecreated
		FROM OPENJSON(@JsonObjectdata, '$.Roomfixtures') WITH (Propertychecklistid BIGINT '$.Propertychecklistid',Propertyhouseroomid BIGINT '$.Propertyhouseroomid',
		Fixtureid INT '$.Fixtureid',Fixtureunits INT '$.Fixtureunits',Fixturestatusid INT '$.Fixturestatusid',Createdby BIGINT '$.Createdby',Datecreated DATETIME2 '$.Datecreated') AS rf
	    CROSS APPLY (
        SELECT Systempropertyhouseid,Systempropertyhouseroomid,Createdby,Datecreated
        FROM OPENJSON(@JsonObjectdata) WITH (Systempropertyhouseid INT '$.Systempropertyhouseid', Systempropertyhouseroomid INT '$.Systempropertyhouseroomid',Createdby BIGINT '$.Createdby',Datecreated DATETIME2 '$.Datecreated')) AS sphf) AS source
		ON target.Propertychecklistid = source.Propertychecklistid AND target.Propertyhouseroomid = source.Systempropertyhouseroomid AND target.Fixtureid = source.Fixtureid
		WHEN MATCHED THEN
        UPDATE SET target.Fixtureunits = source.Fixtureunits,target.Fixturestatusid = source.Fixturestatusid,target.Createdby = source.Createdby,target.Datecreated = source.Datecreated
        WHEN NOT MATCHED BY TARGET THEN
        INSERT (Propertyhouseroomid,Fixtureid,Fixtureunits,Fixturestatusid,Createdby,Datecreated)
		VALUES (source.Systempropertyhouseroomid,source.Fixtureid,source.Fixtureunits,source.Fixturestatusid,source.Createdby,source.Datecreated);

		Set @RespMsg ='Success'
		Set @RespStat =0; 
		COMMIT TRANSACTION;
		Select @RespStat as RespStatus, @RespMsg as RespMessage;

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