CREATE PROCEDURE [dbo].[Usp_Getsystemproductdata]
@Page INT,
@PageSize INT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		   SELECT Productid,Productbarcode,Productname,Productdescription,a.Categoryid,b.Categoryname,a.Brandid,c.Brandname,Sku,Wholesaleprice,Retailprice,Primaryimageurl 
		   FROM Products a
		   INNER JOIN Productcategories b ON a.Categoryid=b.Categoryid
		   INNER JOIN Productbrand c ON a.Brandid=c.Brandid
			--OFFSET @Page ROWS FETCH NEXT @PageSize ROWS ONLY;
			Set @RespMsg ='Ok.'
			Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage

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
