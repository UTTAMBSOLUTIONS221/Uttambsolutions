CREATE PROCEDURE [dbo].[Usp_Getsystemsoftwaretokenspurchasedata]
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		  SELECT PUR.Tokenpurchaseid,PUR.Tokenid,TKN.Tokenname,TKN.Tokenprice,PUR.Userid,PUR.Tokenamount,PUR.Totalcost,PUR.Tokenstatus,PUR.Purchasedate FROM Softwaretokenpurchase PUR INNER JOIN Softwaretoken TKN ON PUR.Tokenid=TKN.Tokenid;
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