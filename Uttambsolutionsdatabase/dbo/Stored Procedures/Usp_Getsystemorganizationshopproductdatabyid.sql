CREATE PROCEDURE [dbo].[Usp_Getsystemorganizationshopproductdatabyid]
@Shopproductid BIGINT,
@ShopproductDetailData VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		   SET @ShopproductDetailData = (SELECT Shopproductid,Productid,Wholesaleprice,Productname,Organizationid,Marketprice,Productdescription,ProductSize,ProductColor,ProductModel,ProductStatus,ProductGender,ProductAgeGroup,ProductMaterial,ProductStock,Islipalater,Productdepositamount,PaymentTerm,Productinterestrate,Periodicamount,DateCreated,
		   (SELECT  Productfeatureid,Shopproductid,Productfeature,DateCreated FROM Shopproductfeature spf WHERE opd.Shopproductid=spf.Shopproductid FOR JSON PATH) AS Shopproductfeature,
		   (SELECT Productwhatsinboxid,Shopproductid,Productwhatsinboxitem,DateCreated FROM Shopproductwhatsinbox spwib WHERE opd.Shopproductid=spwib.Shopproductid FOR JSON PATH) AS Shopproductwhatsinbox,
		   (SELECT Productimagesid,Shopproductid,Productimageurl,DateCreated FROM Shopproductimages spi WHERE opd.Shopproductid=spi.Shopproductid FOR JSON PATH) AS Shopproductimages
		  FROM Organizationshopproducts opd WHERE Shopproductid=@Shopproductid
		   FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
		  )
			Set @RespMsg ='Ok.'
			Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@ShopproductDetailData AS ShopproductDetailData

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
