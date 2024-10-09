CREATE PROCEDURE [dbo].[Usp_Registerhousepropertydata]
@JsonObjectdata VARCHAR(MAX)
AS
BEGIN
   BEGIN
	DECLARE 
			@Houselistingid INT,
			@RespStat int = 0,
			@RespMsg varchar(150) = '';
		  
	BEGIN
		BEGIN TRY	
		--Validate

		BEGIN TRANSACTION;
		    DECLARE @Propertyhouselisting TABLE (Houselistingid INT);

			MERGE INTO Propertyhouselisting AS target
			USING (
			SELECT Houselistingid,Title,Price,Locations,Isforrent,Bedrooms,Bathrooms,Descriptions,Contacts,Extra,Extra1,Extra2,Extra3,Extra4,Extra5,Extra6,Extra7,Extra8,Extra9,Extra10,Isactive,Isdeleted,Createdby,Modifiedby,Datecreated,Datemodified
			FROM OPENJSON(@JsonObjectdata)
			WITH (Houselistingid INT '$.Houselistingid',Title VARCHAR(200) '$.Title',Price DECIMAL(10,2) '$.Price',Locations VARCHAR(400) '$.Locations',Isforrent BIT '$.Isforrent',Bedrooms INT '$.Bedrooms',Bathrooms INT '$.Bathrooms',Descriptions VARCHAR(2000) '$.Descriptions',Contacts VARCHAR(300) '$.Contacts',Extra VARCHAR(200) '$.Extra',Extra1 VARCHAR(200) '$.Extra1',Extra2 VARCHAR(200) '$.Extra2',Extra3 VARCHAR(200) '$.Extra3',
			Extra4 VARCHAR(200) '$.Extra4',Extra5 VARCHAR(200) '$.Extra5',Extra6 VARCHAR(200) '$.Extra6',Extra7 VARCHAR(200) '$.Extra7',Extra8 VARCHAR(200) '$.Extra8',Extra9 VARCHAR(200) '$.Extra9',Extra10 VARCHAR(200) '$.Extra10',Isactive BIT '$.Isactive',Isdeleted BIT '$.Isactive',Blogstatus INT '$.Blogstatus',Createdby INT '$.Createdby',Modifiedby INT '$.Modifiedby',Datecreated DATETIME2 '$.Datecreated',Datemodified DATETIME2 '$.Datemodified')) AS source 
			ON target.Houselistingid = source.Houselistingid
			WHEN MATCHED THEN
			UPDATE SET target.Title=source.Title,target.Price=source.Price,target.Locations=source.Locations,target.Isforrent=source.Isforrent,target.Bedrooms=source.Bedrooms,target.Bathrooms=source.Bathrooms,target.Descriptions=source.Descriptions,target.Contacts=source.Contacts,target.Extra=source.Extra,target.Extra1=source.Extra1,target.Extra2=source.Extra2,target.Extra3=source.Extra3,target.Extra4=source.Extra4,target.Extra5=source.Extra5,target.Extra6=source.Extra6,target.Extra7=source.Extra7,target.Extra8=source.Extra8,target.Extra9=source.Extra9,target.Extra10=source.Extra10,target.Modifiedby=source.Modifiedby,target.Datemodified=source.Datemodified
			WHEN NOT MATCHED BY TARGET THEN
			INSERT (Title,Price,Locations,Isforrent,Bedrooms,Bathrooms,Descriptions,Contacts,Extra,Extra1,Extra2,Extra3,Extra4,Extra5,Extra6,Extra7,Extra8,Extra9,Extra10,Isactive,Isdeleted,Createdby,Modifiedby,Datecreated,Datemodified)
			VALUES (source.Title,source.Price,source.Locations,source.Isforrent,source.Bedrooms,source.Bathrooms,source.Descriptions,source.Contacts,source.Extra,source.Extra1,source.Extra2,source.Extra3,source.Extra4,source.Extra5,source.Extra6,source.Extra7,source.Extra8,source.Extra9,source.Extra10,source.Isactive,source.Isdeleted,source.Createdby,source.Modifiedby,source.Datecreated,source.Datemodified)
		    OUTPUT inserted.Houselistingid INTO @Propertyhouselisting;
		    SET @Houselistingid = (SELECT TOP 1 Houselistingid FROM @Propertyhouselisting);

			MERGE INTO Propertyhouselistingimages AS target
			USING (SELECT ISNULL(@Houselistingid, Houselistingid) AS Houselistingid,Propertyhouseimageurl 
			FROM OPENJSON(@JsonObjectdata, '$.Blogimages')
			WITH (Houselistingid INT '$.Houselistingid',Propertyhouseimageurl VARCHAR(300) '$.Propertyhouseimageurl')) AS source
			ON target.Houselistingid = source.Houselistingid AND target.Propertyhouseimageurl = source.Propertyhouseimageurl
			WHEN MATCHED THEN
			UPDATE SET target.Propertyhouseimageurl = source.Propertyhouseimageurl
			WHEN NOT MATCHED BY TARGET THEN
			INSERT (Houselistingid,Propertyhouseimageurl,Datecreated)
			VALUES (source.Houselistingid,source.Propertyhouseimageurl,GETDATE());
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