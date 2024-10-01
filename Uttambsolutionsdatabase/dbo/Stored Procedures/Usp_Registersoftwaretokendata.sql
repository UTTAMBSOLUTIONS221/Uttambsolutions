CREATE PROCEDURE [dbo].[Usp_Registersoftwaretokendata]
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
			MERGE INTO Softwaretoken AS target
			USING (
			SELECT Tokenid,Tokenname,Tokenprice,Totalsupply,Totalvalue,Datecreated
			FROM OPENJSON(@JsonObjectdata)
			WITH (Tokenid INT '$.Tokenid',Tokenname VARCHAR(100) '$.Tokenname',Tokenprice DECIMAL(10,2) '$.Tokenprice',Totalsupply DECIMAL(18,2) '$.Totalsupply',Totalvalue DECIMAL(18,2)  '$.Totalvalue',Datecreated DATETIME2 '$.Datecreated')) AS source
			ON target.Tokenid = source.Tokenid
			WHEN MATCHED THEN
			UPDATE SET target.Tokenname= source.Tokenname,target.Tokenprice=source.Tokenprice ,target.Totalsupply=source.Totalsupply ,target.Totalvalue=source.Totalvalue
			WHEN NOT MATCHED BY TARGET THEN
			INSERT (Tokenname,Tokenprice,Totalsupply,Totalvalue,Datecreated)
			VALUES (source.Tokenname,source.Tokenprice,source.Totalsupply,source.Totalvalue,source.Datecreated);
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