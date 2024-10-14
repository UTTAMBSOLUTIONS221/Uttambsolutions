CREATE PROCEDURE [dbo].[Usp_Registerparcelcollectioncenterdata]
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
			MERGE INTO Parcelcollectioncenters AS target
			USING (
			SELECT Collectioncenterid,Collectionname,Phonenumber,Operatinghours,Collectionstatus,Managerid
			FROM OPENJSON(@JsonObjectdata)
			WITH (Collectioncenterid BIGINT '$.Collectioncenterid',Collectionname VARCHAR(100) '$.Collectionname',Phonenumber VARCHAR(15) '$.Phonenumber',Operatinghours VARCHAR(50) '$.Operatinghours',Collectionstatus INT '$.Collectionstatus',Managerid INT '$.Managerid'
			)) AS source
			ON target.Collectioncenterid = source.Collectioncenterid
			WHEN MATCHED THEN
			UPDATE SET target.Collectionname = source.Collectionname,target.Phonenumber =source.Phonenumber,target.Operatinghours =source.Operatinghours,target.Collectionstatus =source.Collectionstatus
			WHEN NOT MATCHED BY TARGET THEN
			INSERT (Collectionname,Phonenumber,Operatinghours,Collectionstatus,Managerid)
			VALUES (source.Collectionname,source.Phonenumber,source.Operatinghours,source.Collectionstatus,source.Managerid);
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