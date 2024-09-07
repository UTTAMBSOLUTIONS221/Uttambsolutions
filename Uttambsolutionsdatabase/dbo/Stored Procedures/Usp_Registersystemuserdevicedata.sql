CREATE PROCEDURE [dbo].[Usp_Registersystemuserdevicedata]
@JsonObjectdata VARCHAR(MAX)
AS
BEGIN
   BEGIN
	DECLARE @RespStat int = 0,
			@RespMsg varchar(150) = '';
		  
	BEGIN
		BEGIN TRY	
		--Validate

		BEGIN TRANSACTION;
			MERGE INTO Systemuserdevices AS target
			USING (
			SELECT Systemuserdeviceid,Userid,Androidid,Manufacturer,Model,Osversion,Platforms,Devicename,Datecreated,Datemodified
			FROM OPENJSON(@JsonObjectdata)
			WITH (Systemuserdeviceid INT '$.Systemuserdeviceid',Userid BIGINT '$.Userid',Androidid VARCHAR(20) '$.Androidid',
			Manufacturer VARCHAR(20) '$.Manufacturer',Model VARCHAR(20) '$.Model',Osversion VARCHAR(20) '$.Osversion',Platforms VARCHAR(20) '$.Platforms',Devicename VARCHAR(20) '$.Devicename',Datecreated DATETIME2 '$.Datecreated',Datemodified DATETIME2 '$.Datemodified'
			)) AS source
			ON target.Model = source.Model AND target.Androidid = source.Androidid 
			WHEN MATCHED  AND source.Userid!=0 THEN
			UPDATE SET target.Userid = source.Userid,target.Datemodified = source.Datemodified
			WHEN NOT MATCHED BY TARGET THEN
			INSERT (Userid,Androidid,Manufacturer,Model,Osversion,Platforms,Devicename,Datecreated,Datemodified)
			VALUES (source.Userid,source.Androidid,source.Manufacturer,source.Model,source.Osversion,source.Platforms,source.Devicename,source.Datecreated,source.Datemodified);
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