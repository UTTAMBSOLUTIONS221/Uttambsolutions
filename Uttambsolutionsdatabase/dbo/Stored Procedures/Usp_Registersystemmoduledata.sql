CREATE PROCEDURE [dbo].[Usp_Registersystemmoduledata]
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
			MERGE INTO Systemmodules AS target
			USING (
			SELECT Moduleid,Modulename,Moduledescription,Slug,Moduleimagepath
			FROM OPENJSON(@JsonObjectdata)
			WITH (Moduleid BIGINT '$.Moduleid',Modulename VARCHAR(100) '$.Modulename',Moduledescription VARCHAR(400) '$.Moduledescription',Slug VARCHAR(100) '$.Slug',Moduleimagepath VARCHAR(200) '$.Moduleimagepath'
			)) AS source
			ON target.Moduleid = source.Moduleid
			WHEN MATCHED THEN
			UPDATE SET target.Modulename = source.Modulename,target.Moduledescription = source.Moduledescription,target.Slug = source.Slug,target.Moduleimagepath = source.Moduleimagepath 
			WHEN NOT MATCHED BY TARGET THEN
			INSERT (Modulename,Moduledescription,Slug,Moduleimagepath)
			VALUES (source.Modulename,source.Moduledescription,source.Slug,source.Moduleimagepath);
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
