CREATE PROCEDURE [dbo].[Usp_Registersystemcategorydata]
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
			MERGE INTO Productcategories AS target
			USING (
			SELECT  Categoryid,Categoryname,Parentcategoryname,Vatrate,Imageurl
			FROM OPENJSON(@JsonObjectdata)
			WITH (Categoryid BIGINT '$.Categoryid',Categoryname VARCHAR(100) '$.Categoryname',Parentcategoryname VARCHAR(400) '$.Parentcategoryname',Vatrate Decimal(10,2) '$.Vatrate',Imageurl VARCHAR(200) '$.Imageurl'
			)) AS source
			ON target.Categoryid = source.Categoryid
			WHEN MATCHED THEN
			UPDATE SET target.Categoryname = source.Categoryname,target.Parentcategoryname = source.Parentcategoryname,target.Vatrate = source.Vatrate WHEN NOT MATCHED BY TARGET THEN
			INSERT (Categoryname,Parentcategoryname,Vatrate,Imageurl)
			VALUES (source.Categoryname,source.Parentcategoryname,source.Vatrate,source.Imageurl);
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
