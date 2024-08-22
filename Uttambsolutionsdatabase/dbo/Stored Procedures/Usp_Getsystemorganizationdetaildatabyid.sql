CREATE PROCEDURE [dbo].[Usp_Getsystemorganizationdetaildatabyid]
    @Organizationid BIGINT,
	@OrganizationDetailData VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		--validate	

		BEGIN TRANSACTION;
		  SET @OrganizationDetailData = (
		  SELECT org.Organizationid,org.Organizationowner,org.Organizationname,org.OrganizationPhone,org.OrganizationEmail,org.Organizationdescription,org.Organizationtypeid,org.Organizationstatus,org.Datecreated,
		  (SELECT Prd.Productid,Prd.Productbarcode,Prd.Productname,Prd.Productdescription,Prd.Categoryid,Prc.Categoryname,Prd.Brandid,Prb.Brandname,Prd.Sku,Prd.Wholesaleprice,Prd.Retailprice,Prd.Primaryimageurl 
		   FROM Products Prd
		   INNER JOIN Productcategories Prc ON Prd.Categoryid=Prc.Categoryid
		   INNER JOIN Productbrand Prb ON Prd.Brandid=Prb.Brandid FOR JSON PATH) AS Systemproducts,
		   (SELECT Shopproductid,opd.Productid,spr.Primaryimageurl,opd.Productname,Organizationid,Marketprice,opd.Productdescription,ProductSize,ProductColor,ProductModel,ProductStock,DateCreated,
		   (SELECT  Productfeatureid,Shopproductid,Productfeature,DateCreated FROM Shopproductfeature spf WHERE opd.Shopproductid=spf.Shopproductid FOR JSON PATH) AS Shopproductfeature,
		   (SELECT Productwhatsinboxid,Shopproductid,Productwhatsinboxitem,DateCreated FROM Shopproductwhatsinbox spwib WHERE opd.Shopproductid=spwib.Shopproductid FOR JSON PATH) AS Shopproductwhatsinbox,
		   (SELECT Productimagesid,Shopproductid,Productimageurl,DateCreated FROM Shopproductimages spi WHERE opd.Shopproductid=spi.Shopproductid FOR JSON PATH) AS Shopproductimages
		   FROM Organizationshopproducts opd
		   INNER JOIN Products spr ON opd.Productid=spr.Productid FOR JSON PATH) AS Organizationshopproducts
		  FROM Systemorganizations org WHERE org.Organizationid=@Organizationid
		  FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
		  )
	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;
           SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@OrganizationDetailData AS OrganizationDetailData  
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
