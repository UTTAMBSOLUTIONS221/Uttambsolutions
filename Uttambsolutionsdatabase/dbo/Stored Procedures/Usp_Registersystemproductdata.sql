CREATE PROCEDURE [dbo].[Usp_Registersystemproductdata]
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
			MERGE INTO Products AS target
			USING (
			SELECT Productid,Productbarcode,Productname,Productdescription,Categoryid,Brandid,Sku,Wholesaleprice,Retailprice,Primaryimageurl
			FROM OPENJSON(@JsonObjectdata)
			WITH (Productid BIGINT '$.Productid',Productbarcode VARCHAR(100) '$.Productbarcode',Productname VARCHAR(100) '$.Productname',Productdescription VARCHAR(4000) '$.Productdescription',Categoryid INT '$.Categoryid',Brandid INT '$.Brandid',Sku VARCHAR(100) '$.Sku',Wholesaleprice DECIMAL(18,2) '$.Wholesaleprice',Retailprice DECIMAL(18,2) '$.Retailprice',Primaryimageurl VARCHAR(200) '$.Primaryimageurl'
			)) AS source
			ON target.Productid = source.Productid
			WHEN MATCHED THEN
			UPDATE SET target.Productbarcode = source.Productbarcode,target.Productname = source.Productname,target.Productdescription = source.Productdescription,
			target.Categoryid = source.Categoryid,target.Brandid = source.Brandid,target.Sku = source.Sku,target.Wholesaleprice = source.Wholesaleprice,target.Retailprice = source.Retailprice WHEN NOT MATCHED BY TARGET THEN
			INSERT (Productbarcode,Productname,Productdescription,Categoryid,Brandid,Sku,Wholesaleprice,Retailprice,Primaryimageurl)
			VALUES (source.Productbarcode,source.Productname,source.Productdescription,source.Categoryid,source.Brandid,source.Sku,source.Wholesaleprice,source.Retailprice,source.Primaryimageurl);
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
