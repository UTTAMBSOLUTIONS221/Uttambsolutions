CREATE PROCEDURE [dbo].[Usp_Registersoftwaretokenpurchasedata]
@JsonObjectdata VARCHAR(MAX)
AS
BEGIN
   BEGIN
	DECLARE @Tokenprice DECIMAL(10,2),
	        @Totalcost DECIMAL(10,2),
			@Tokenamount DECIMAL(10,2),
			@RespStat int = 0,
			@RespMsg varchar(150) = '';
		  
	BEGIN
		BEGIN TRY	
		--Validate
		IF NOT EXISTS(SELECT Tokenid FROM Softwaretoken WHERE Tokenid = JSON_VALUE(@JsonObjectData, '$.Tokenid'))
		BEGIN
			SELECT 1 AS RespStatus, 'Token does not exist!' AS RespMessage;
			RETURN;
		END

		BEGIN TRANSACTION;
		    SELECT @Tokenprice = Tokenprice FROM Softwaretoken WHERE Tokenid = JSON_VALUE(@JsonObjectData, '$.Tokenid');
			SET @Totalcost=@Tokenprice * JSON_VALUE(@JsonObjectData, '$.Tokenamount');
			MERGE INTO Softwaretokenpurchase AS target
			USING (
			SELECT Tokenpurchaseid,Tokenid,Userid,Tokenamount,@Totalcost AS Totalcost,Purchasedate
			FROM OPENJSON(@JsonObjectdata)
			WITH (Tokenpurchaseid INT '$.Tokenpurchaseid',Tokenid INT '$.Tokenid',Userid INT '$.Userid',Tokenamount DECIMAL(10,2) '$.Tokenamount',Totalcost DECIMAL(10,2) '$.Totalcost',Purchasedate DATETIME2 '$.Purchasedate')) AS source
			ON target.Tokenpurchaseid = source.Tokenpurchaseid AND target.Tokenid = source.Tokenid AND target.Userid = source.Userid
			WHEN MATCHED THEN
			UPDATE SET target.Totalcost= source.Totalcost
			WHEN NOT MATCHED BY TARGET THEN
			INSERT (Tokenid,Userid,Tokenamount,Totalcost,Purchasedate)
			VALUES (source.Tokenid,source.Userid,source.Tokenamount,source.Totalcost,source.Purchasedate);


			--MERGE INTO Tokenownership AS target
			--USING (
			--SELECT Tokenownershipid,Tokenid,Userid,Tokenamount
			--FROM OPENJSON(@JsonObjectdata)
			--WITH (Tokenownershipid INT '$.Tokenownershipid',Tokenid INT '$.Tokenid',Userid INT '$.Userid',Tokenamount DECIMAL(10,2) '$.Tokenamount')) AS source
			--ON  target.Tokenid = source.Tokenid AND target.Userid = source.Userid
			--WHEN MATCHED THEN
			--UPDATE SET target.Tokenamount= target.Tokenamount+source.Tokenamount
			--WHEN NOT MATCHED BY TARGET THEN
			--INSERT (Tokenid,Userid,Tokenamount)
			--VALUES (source.Tokenid,source.Userid,source.Tokenamount);

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