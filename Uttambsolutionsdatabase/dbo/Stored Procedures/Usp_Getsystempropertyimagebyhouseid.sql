CREATE PROCEDURE [dbo].[Usp_Getsystempropertyimagebyhouseid]
@Houseid INT,
@Systempropertyhouseimagedata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		 SET @Systempropertyhouseimagedata= 
			(SELECT(SELECT @Houseid AS Propertyhouseid,
			(SELECT SPHID.Propertyimageid,SPHID.Propertyhouseid,SPHID.Houseorroom,SPHID.Houseorroomimageurl,SPHID.Createdby,SPHID.Datecreated FROM Systempropertyhouseimages SPHID WHERE SPHID.Propertyhouseid =@Houseid AND SPHID.Houseorroom ='PropertyHouse' FOR JSON PATH) AS PropertyHouseImage
			FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			)AS Data
			FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			);

	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systempropertyhouseimagedata AS Systempropertyhouseimagedata;

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