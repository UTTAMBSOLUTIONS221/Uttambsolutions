CREATE PROCEDURE [dbo].[Usp_Registersystemblogcategorydata]
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
			MERGE INTO Systemblogcategories AS target
			USING (
			SELECT  Blogcategoryid,Blogcategoryname,Datecreated
			FROM OPENJSON(@JsonObjectdata)
			WITH (Blogcategoryid BIGINT '$.Blogcategoryid',Blogcategoryname VARCHAR(100) '$.Blogcategoryname',Datecreated DATETIME '$.Datecreated'
			)) AS source
			ON target.Blogcategoryid = source.Blogcategoryid
			WHEN MATCHED THEN
			UPDATE SET target.Blogcategoryname = source.Blogcategoryname WHEN NOT MATCHED BY TARGET THEN
			INSERT (Blogcategoryname,Datecreated)
			VALUES (source.Blogcategoryname,source.Datecreated);
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
