CREATE PROCEDURE [dbo].[Usp_Registersystemproductbranddata]
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
			MERGE INTO Productbrand AS target
			USING (
			SELECT  Brandid,Brandname,Brandpathurl
			FROM OPENJSON(@JsonObjectdata)
			WITH (Brandid BIGINT '$.Brandid',Brandname VARCHAR(100) '$.Brandname',Brandpathurl VARCHAR(240) '$.Brandpathurl'
			)) AS source
			ON target.Brandid = source.Brandid
			WHEN MATCHED THEN
			UPDATE SET target.Brandname = source.Brandname WHEN NOT MATCHED BY TARGET THEN
			INSERT (Brandname,Brandpathurl)
			VALUES (source.Brandname,source.Brandpathurl);
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
