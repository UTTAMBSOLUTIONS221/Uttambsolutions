CREATE PROCEDURE [dbo].[Usp_Getsystemorganizationshopproductsdata]
@Organizationshopproductsdata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		SET @Organizationshopproductsdata= 
		(SELECT
		(SELECT orgshopproduct.Shopproductid,orgshopproduct.Productid,orgshopproduct.Organizationid,sst.Firstname+' '+sst.Lastname AS Fullname,sysorg.Organizationname,sysorg.OrganizationEmail,
		(CASE WHEN orgshopproduct.ProductStock<2 THEN 'out of stock' ELSE 'in stock' END) AS Productavailability,
		sysorg.OrganizationPhone,sysorg.Organizationdescription,orgshopproduct.Productname,orgshopproduct.Productdescription,
		prd.Primaryimageurl,prd.Productdescription AS Mainproductdescription,prd.Productbarcode,prdc.Categoryname,prdc.Parentcategoryname,Prdb.Brandname,
		prd.Sku,prd.Brandid,prd.Categoryid,prd.Wholesaleprice,prd.Retailprice,prdc.Vatrate,prdc.Imageurl,orgshopproduct.ProductSize,
		orgshopproduct.ProductColor,orgshopproduct.ProductModel,orgshopproduct.ProductStatus,orgshopproduct.ProductGender,orgshopproduct.ProductAgeGroup,orgshopproduct.ProductMaterial,orgshopproduct.ProductStock,orgshopproduct.Islipalater,orgshopproduct.Productdepositamount,
		orgshopproduct.PaymentTerm,orgshopproduct.Productinterestrate,orgshopproduct.Periodicamount,orgshopproduct.Marketprice,sysorg.Organizationstatus,orgshopproduct.DateCreated,
		(SELECT  Productfeatureid,Shopproductid,Productfeature,DateCreated FROM Shopproductfeature spf WHERE orgshopproduct.Shopproductid=spf.Shopproductid FOR JSON PATH) AS Productfeatures,
		(SELECT Productwhatsinboxid,Shopproductid,Productwhatsinboxitem,DateCreated FROM Shopproductwhatsinbox spwib WHERE orgshopproduct.Shopproductid=spwib.Shopproductid FOR JSON PATH) AS Productwhatsinbox,
		(SELECT Productimagesid,Shopproductid,Productimageurl,DateCreated FROM Shopproductimages spi WHERE orgshopproduct.Shopproductid=spi.Shopproductid FOR JSON PATH) AS Productotherimages
		FROM Organizationshopproducts orgshopproduct
		INNER JOIN Systemorganizations sysorg ON orgshopproduct.Organizationid=sysorg.Organizationid
		INNER JOIN Products prd ON orgshopproduct.Productid=prd.Productid
		INNER JOIN Productcategories prdc ON prd.Categoryid=prdc.Categoryid
		INNER JOIN Productbrand Prdb ON prd.Brandid=Prdb.Brandid
		INNER JOIN Systemstaffs sst ON sysorg.Organizationowner=sst.Userid
		WHERE sysorg.Organizationstatus=0
		FOR JSON PATH
		) AS Data
		FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
		);
	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Organizationshopproductsdata AS Organizationshopproductsdata;

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
