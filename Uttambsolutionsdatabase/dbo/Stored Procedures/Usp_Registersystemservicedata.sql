CREATE PROCEDURE [dbo].[Usp_Registersystemservicedata]
@JsonObjectdata VARCHAR(MAX)
AS
BEGIN
   BEGIN
	DECLARE @RespStat int = 0,
			@RespMsg varchar(150) = '',
			@Serviceid BIGINT;
		  
	BEGIN
		BEGIN TRY	
		--Validate

		BEGIN TRANSACTION;
		 DECLARE @Systemservicesdata TABLE (Serviceid BIGINT);
			MERGE INTO Systemservices AS target
			USING (
			SELECT Serviceid,Servicename,Subscriptionfee,Isvisible
			FROM OPENJSON(@JsonObjectdata)
			WITH (Serviceid INT '$.Serviceid',Servicename VARCHAR(80) '$.Servicename',Subscriptionfee DECIMAL(10,2) '$.Subscriptionfee',Isvisible BIT '$.Isvisible'
			)) AS source
			ON target.Serviceid = source.Serviceid
			WHEN MATCHED THEN
			UPDATE SET target.Servicename = source.Servicename,target.Subscriptionfee = source.Subscriptionfee 
			WHEN NOT MATCHED BY TARGET THEN
			INSERT (Servicename,Subscriptionfee,Isvisible)
			VALUES (source.Servicename,source.Subscriptionfee,source.Isvisible)
		    OUTPUT inserted.Serviceid INTO @Systemservicesdata;

        -- Set @Jobid to the new Jobid if an insert occurred
          SET @Serviceid = (SELECT TOP 1 Serviceid FROM @Systemservicesdata);

		  MERGE INTO Systemservicesitems AS target
		  USING (
		  SELECT Serviceitemid,ISNULL(Serviceid,@Serviceid) AS Serviceid,Serviceitemname,Serviceitemimageurl
		 	FROM OPENJSON(@JsonObjectdata, '$.Serviceitems')
		  WITH (Serviceitemid INT '$.Serviceitemid',Serviceid INT '$.Serviceid',Serviceitemname VARCHAR(200) '$.Serviceitemname',Serviceitemimageurl VARCHAR(200) '$.Serviceitemimageurl'
		  )) AS source
		  ON target.Serviceitemid = source.Serviceitemid AND target.Serviceid = source.Serviceid
		  WHEN MATCHED THEN
		  UPDATE SET target.Serviceitemname = source.Serviceitemname,target.Serviceitemimageurl = source.Serviceitemimageurl 
		  WHEN NOT MATCHED BY TARGET THEN
		  INSERT (Serviceid,Serviceitemname,Serviceitemimageurl)
		  VALUES (source.Serviceid,source.Serviceitemname,source.Serviceitemimageurl);

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