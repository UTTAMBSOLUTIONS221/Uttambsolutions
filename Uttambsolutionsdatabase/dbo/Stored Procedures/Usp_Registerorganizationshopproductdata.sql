CREATE PROCEDURE [dbo].[Usp_Registerorganizationshopproductdata]
@JsonObjectdata VARCHAR(MAX),
@Organizationshopproductsdata VARCHAR(MAX)  OUTPUT
AS
BEGIN
    DECLARE @RespStat int = 0,
            @RespMsg varchar(150) = '',
			@NewShopproductid BIGINT = NULL,
			@ExistingShopproductid BIGINT = NULL;
    BEGIN TRY
        -- Validate
		IF((SELECT Shopproductid FROM OPENJSON(@JsonObjectdata) WITH (Shopproductid BIGINT '$.Shopproductid'))<1)
		BEGIN
			IF EXISTS(SELECT Shopproductid FROM Organizationshopproducts WHERE Productid= (SELECT Productid FROM OPENJSON(@JsonObjectdata) WITH (Productid BIGINT '$.Productid')) AND Organizationid= (SELECT Organizationid FROM OPENJSON(@JsonObjectdata) WITH (Organizationid BIGINT '$.Organizationid')))
			BEGIN
			  SELECT 1 AS RespStatus, 'Product already added to shop' AS RespMessage;
			return;
			END
		END

        BEGIN TRANSACTION;
		DECLARE @Shopproductdata TABLE (Shopproductid BIGINT)
		  SET @ExistingShopproductid = (SELECT Shopproductid FROM OPENJSON(@JsonObjectdata) WITH (Shopproductid BIGINT '$.Shopproductid'));

            -- Merge into Organizationshopproducts and capture inserted ID
            MERGE INTO Organizationshopproducts AS target
            USING (
                SELECT Shopproductid, Productid,Wholesaleprice, Productname, Organizationid, Marketprice, Productdescription, ProductSize, ProductColor, ProductModel,ProductStatus,ProductGender,ProductAgeGroup,ProductMaterial,ProductStock,Islipalater,Productdepositamount,PaymentTerm,Productinterestrate,Periodicamount, DateCreated
                FROM OPENJSON(@JsonObjectdata)
                WITH (
                    Shopproductid BIGINT '$.Shopproductid',
                    Productid BIGINT '$.Productid',
					Wholesaleprice DECIMAL(18, 2) '$.Wholesaleprice',
                    Productname VARCHAR(100) '$.Productname',
                    Organizationid BIGINT '$.Organizationid',
                    Marketprice DECIMAL(18, 2) '$.Marketprice',
                    Productdescription VARCHAR(255) '$.Productdescription',
                    ProductSize VARCHAR(50) '$.ProductSize',
                    ProductColor VARCHAR(50) '$.ProductColor',
					ProductModel VARCHAR(50) '$.ProductModel',
					ProductStatus VARCHAR(50) '$.ProductStatus',
					ProductGender VARCHAR(50) '$.ProductGender',
					ProductAgeGroup VARCHAR(50) '$.ProductAgeGroup',
					ProductMaterial VARCHAR(100) '$.ProductMaterial',
                    ProductStock DECIMAL(18, 2) '$.ProductStock',
					Islipalater BIT '$.Islipalater',
					Productdepositamount DECIMAL(18, 2) '$.Productdepositamount',
					PaymentTerm INT '$.PaymentTerm',
					Productinterestrate DECIMAL(18, 2) '$.Productinterestrate',
					Periodicamount DECIMAL(18, 2) '$.Periodicamount',
                    DateCreated DATETIME2 '$.DateCreated'
                )
            ) AS source
            ON target.Shopproductid = source.Shopproductid
            WHEN MATCHED THEN
                UPDATE SET 
                    target.Productname = source.Productname,
					target.Wholesaleprice = source.Wholesaleprice,
                    target.Marketprice = source.Marketprice,
                    target.Productdescription = source.Productdescription,
                    target.ProductSize = source.ProductSize,
                    target.ProductColor = source.ProductColor,
                    target.ProductModel = source.ProductModel,
					target.ProductStatus = source.ProductStatus,
					target.ProductGender = source.ProductGender ,
					target.ProductAgeGroup = source.ProductAgeGroup ,
					target.ProductMaterial = source.ProductMaterial,
                    target.ProductStock = source.ProductStock,
					target.Islipalater = source.Islipalater,
					target.Productdepositamount = source.Productdepositamount,
					target.PaymentTerm = source.PaymentTerm,
					target.Productinterestrate = source.Productinterestrate,
					target.Periodicamount = source.Periodicamount,
                    target.DateCreated = source.DateCreated
			    WHEN NOT MATCHED BY TARGET THEN
                INSERT (Productid, Wholesaleprice,Productname, Organizationid, Marketprice, Productdescription, ProductSize, ProductColor, ProductModel, ProductStatus,ProductGender,ProductAgeGroup,ProductMaterial,ProductStock,Islipalater,Productdepositamount,PaymentTerm,Productinterestrate,Periodicamount, DateCreated)
                VALUES (source.Productid,source.Wholesaleprice, source.Productname, source.Organizationid, source.Marketprice, source.Productdescription, source.ProductSize, source.ProductColor, source.ProductModel,source.ProductStatus,source.ProductGender,source.ProductAgeGroup,source.ProductMaterial, source.ProductStock, source.Islipalater,source.Productdepositamount,source.PaymentTerm,source.Productinterestrate,source.Periodicamount, source.DateCreated)
                OUTPUT inserted.Shopproductid INTO @Shopproductdata;
				 -- Set @NewShopproductid to the new Shopproductid if an insert occurred
				SET @NewShopproductid = (SELECT TOP 1 Shopproductid FROM @Shopproductdata);

				-- If no insert occurred, use the existing Shopproductid
				IF @NewShopproductid IS NULL SET @NewShopproductid = @ExistingShopproductid;
            -- Insert into Shopproductfeatures
            MERGE INTO Shopproductfeature AS target
			USING (
				SELECT 
				   source.ProductFeatureId,
					ISNULL(@NewShopproductid, source.Shopproductid) AS Shopproductid,
					source.ProductFeature,
					source.DateCreated
				FROM OPENJSON(@JsonObjectdata, '$.Shopproductfeature')
				WITH (
				    ProductFeatureId BIGINT '$.ProductFeatureId',
					Shopproductid BIGINT '$.Shopproductid',
					ProductFeature VARCHAR(255) '$.ProductFeature',
					DateCreated DATETIME2 '$.DateCreated'
				) AS source
			) AS sourceData
			ON target.ProductFeatureId = sourceData.ProductFeatureId 
			   AND target.ProductFeature = sourceData.ProductFeature
			WHEN MATCHED THEN
				UPDATE SET target.ProductFeature = sourceData.ProductFeature,target.DateCreated = sourceData.DateCreated
			WHEN NOT MATCHED THEN
				INSERT (Shopproductid, ProductFeature, DateCreated)
				VALUES (sourceData.Shopproductid, sourceData.ProductFeature, sourceData.DateCreated);

           MERGE INTO Shopproductwhatsinbox AS target
			USING (
				SELECT 
				    source.ProductWhatsInBoxId,
					 ISNULL(@NewShopproductid, source.Shopproductid) AS Shopproductid,
					source.ProductWhatsInBoxItem,
					source.DateCreated
				FROM OPENJSON(@JsonObjectdata, '$.Shopproductwhatsinbox')
				WITH (
				    ProductWhatsInBoxId BIGINT '$.ProductWhatsInBoxId',
					Shopproductid BIGINT '$.Shopproductid',
					ProductWhatsInBoxItem VARCHAR(255) '$.ProductWhatsInBoxItem',
					DateCreated DATETIME2 '$.DateCreated'
				) AS source
			) AS sourceData
			ON target.ProductWhatsInBoxId = sourceData.ProductWhatsInBoxId 
			   AND target.ProductWhatsInBoxItem = sourceData.ProductWhatsInBoxItem
			WHEN MATCHED THEN
				UPDATE SET target.ProductWhatsInBoxItem = sourceData.ProductWhatsInBoxItem,
					target.DateCreated = sourceData.DateCreated
			WHEN NOT MATCHED THEN
				INSERT (Shopproductid, ProductWhatsInBoxItem, DateCreated)
				VALUES (sourceData.Shopproductid, sourceData.ProductWhatsInBoxItem, sourceData.DateCreated);

            -- Insert into ShopProductImages
            MERGE INTO Shopproductimages AS target
			USING (
				SELECT
				    source.ProductImagesId,
					ISNULL(@NewShopproductid, source.Shopproductid) AS Shopproductid,
					source.Productimageurl,
					source.DateCreated
				FROM OPENJSON(@JsonObjectdata, '$.ShopProductImage')
				WITH (
				    ProductImagesId BIGINT '$.ProductImagesId',
					Shopproductid BIGINT '$.Shopproductid',
					Productimageurl VARCHAR(500) '$.ProductImageUrl',
					DateCreated DATETIME2 '$.DateCreated'
				) AS source
			) AS sourceData
			ON target.ProductImagesId = sourceData.ProductImagesId 
			   AND target.Productimageurl = sourceData.Productimageurl
			WHEN MATCHED THEN
				UPDATE SET target.Productimageurl = sourceData.Productimageurl,
					target.DateCreated = sourceData.DateCreated
			WHEN NOT MATCHED THEN
				INSERT (Shopproductid, Productimageurl, DateCreated)
				VALUES (sourceData.Shopproductid, sourceData.Productimageurl, sourceData.DateCreated);

	   SET @Organizationshopproductsdata= (SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage,orgshopproduct.Shopproductid,orgshopproduct.Productid,orgshopproduct.Organizationid,sst.Firstname+' '+sst.Lastname AS Fullname,sysorg.Organizationname,sysorg.OrganizationEmail,
		(CASE WHEN orgshopproduct.ProductStock<2 THEN 'out of stock' ELSE 'in stock' END) AS Productavailability,
		sysorg.OrganizationPhone,sysorg.Organizationdescription,orgshopproduct.Productname,orgshopproduct.Productdescription,
		prd.Primaryimageurl,prd.Productdescription AS Mainproductdescription,prd.Productbarcode,prdc.Categoryname,prdc.Parentcategoryname,Prdb.Brandname,
		prd.Sku,prd.Brandid,prd.Categoryid,prd.Wholesaleprice,prd.Retailprice,prdc.Vatrate,prdc.Imageurl,orgshopproduct.ProductSize,
		orgshopproduct.ProductColor,orgshopproduct.ProductModel,orgshopproduct.ProductStatus,orgshopproduct.ProductGender,orgshopproduct.ProductAgeGroup,orgshopproduct.ProductMaterial,orgshopproduct.ProductStock,orgshopproduct.Islipalater,orgshopproduct.Productdepositamount,
		orgshopproduct.PaymentTerm,orgshopproduct.Productinterestrate,orgshopproduct.Periodicamount,orgshopproduct.Marketprice,sysorg.Organizationstatus,orgshopproduct.DateCreated
		FROM Organizationshopproducts orgshopproduct
		INNER JOIN Systemorganizations sysorg ON orgshopproduct.Organizationid=sysorg.Organizationid
		INNER JOIN Products prd ON orgshopproduct.Productid=prd.Productid
		INNER JOIN Productcategories prdc ON prd.Categoryid=prdc.Categoryid
		INNER JOIN Productbrand Prdb ON prd.Brandid=Prdb.Brandid
		INNER JOIN Systemstaffs sst ON sysorg.Organizationowner=sst.Userid
		WHERE orgshopproduct.Shopproductid=@NewShopproductid
		FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
		);

            SET @RespMsg = 'Success';
            SET @RespStat = 0;

            COMMIT TRANSACTION;
            SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage,@Organizationshopproductsdata AS Organizationshopproductsdata;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT 'Error: ' + ERROR_MESSAGE();
        SELECT 2 AS RespStatus, '0 - Error(s) Occurred: ' + ERROR_MESSAGE() AS RespMessage;
    END CATCH;

    SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;
    RETURN;
END
