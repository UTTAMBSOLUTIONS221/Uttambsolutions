CREATE PROCEDURE [dbo].[Usp_Registerstoreproductdata]
@JsonObjectdata VARCHAR(MAX)
AS
BEGIN
   BEGIN
	DECLARE 
			@Storeitemid INT,
			@RespStat int = 0,
			@RespMsg varchar(150) = '';
		  
	BEGIN
		BEGIN TRY	
		--Validate

		BEGIN TRANSACTION;
		    DECLARE @Systemstoreitemsdata TABLE (Storeitemid INT);

			MERGE INTO Systemstoreitems AS target
			USING (
			SELECT Storeitemid,Storeitemname ,Itembrandname,Itemsize ,Itembuyingprice,Itemsellingprice,Itemstatus,Isactive,Isdeleted,Createdby,Modifiedby,Datecreated,Datemodified
			FROM OPENJSON(@JsonObjectdata)
			WITH (Storeitemid INT '$.Storeitemid',Storeitemname VARCHAR(100) '$.Storeitemname',Itembrandname VARCHAR(70) '$.Itembrandname',Itemsize VARCHAR(70) '$.Itemsize',Itembuyingprice DECIMAL(10,2) '$.Itembuyingprice',Itemsellingprice DECIMAL(10,2) '$.Itemsellingprice',Itemstatus INT '$.Itemstatus',Isactive BIT '$.Isactive',Isdeleted BIT '$.Isdeleted',Createdby INT '$.Createdby',Modifiedby INT '$.Modifiedby',Datecreated DATETIME2 '$.Datecreated',Datemodified DATETIME2 '$.Datemodified')) AS source 
			ON target.Storeitemid = source.Storeitemid
			WHEN MATCHED THEN
			UPDATE SET target.Storeitemname =source.Storeitemname,target.Itembrandname =source.Itembrandname,target.Itemsize =source.Itemsize,target.Itembuyingprice =source.Itembuyingprice,target.Itemsellingprice =source.Itemsellingprice,target.Modifiedby =source.Modifiedby,target.Datemodified =source.Datemodified
			WHEN NOT MATCHED BY TARGET THEN
			INSERT (Storeitemname ,Itembrandname ,Itemsize,Itembuyingprice,Itemsellingprice,Itemstatus,Isactive,Isdeleted,Createdby,Modifiedby,Datecreated,Datemodified)
			VALUES (source.Storeitemname,source.Itembrandname,source.Itemsize,source.Itembuyingprice,source.Itemsellingprice,source.Itemstatus,source.Isactive,source.Isdeleted,source.Createdby,source.Modifiedby,source.Datecreated,source.Datemodified)
		    OUTPUT inserted.Storeitemid INTO @Systemstoreitemsdata;
		    SET @Storeitemid = (SELECT TOP 1 Storeitemid FROM @Systemstoreitemsdata);

			MERGE INTO Systemstoreitemsimages AS target
			USING (SELECT ISNULL(@Storeitemid, Storeitemid) AS Storeitemid,Storeproductimgurl,Datecreated 
			FROM OPENJSON(@JsonObjectdata, '$.Storeproductimages')
			WITH (Storeitemid INT '$.Storeitemid',Storeproductimgurl VARCHAR(300) '$.Storeproductimgurl',Datecreated DATETIME2 '$.Datecreated')) AS source
			ON target.Storeitemid = source.Storeitemid AND target.Storeproductimgurl = source.Storeproductimgurl
			WHEN MATCHED THEN
			UPDATE SET target.Storeproductimgurl = source.Storeproductimgurl
			WHEN NOT MATCHED BY TARGET THEN
			INSERT (Storeitemid,Storeproductimgurl,Datecreated)
			VALUES (source.Storeitemid,source.Storeproductimgurl,source.Datecreated);
		
		
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