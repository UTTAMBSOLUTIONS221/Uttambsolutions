CREATE PROCEDURE [dbo].[Usp_Getallsystempropertyvacanthousesdata]
@Systempropertydata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		  SET @Systempropertydata= 
				  (SELECT (SELECT Systempropertyhouseroom.Systempropertyhouseroomid,
					Systemstaff.Firstname+''+Systemstaff.Lastname AS Propertyhouseownername,
					Systemcounty.Countyname,Systemsubcounty.Subcountyname,Systemsubcountyward.Subcountywardname,
					Systempropertyhouse.Streetorlandmark,
					CASE WHEN Systempropertyhouse.Propertyhousestatus=0 THEN 'First Tenants' ELSE 'Subsequent Tenants' END  AS Propertyhousestatusdata
					,Systempropertyhouseroom.Systempropertyhousesizename,
				     Systemhousesize.Systemhousesizename,
					Systempropertyhouseroom.Systempropertyhousesizerent,
					Systempropertyhouseroom.Systempropertyhousesizedeposit,
					Systempropertyhouse.Propertyhousename,
					Systempropertyhouseroom.Isvacant,
					Systempropertyhouse.Hashousewatermeter,
					Systempropertyhouseroom.Forcaretaker,
					CASE WHEN Systempropertyhouseroom.Isvacant=0 THEN 'No' ELSE 'Yes' END  AS Propertyhousevacant,
					CASE WHEN Systempropertyhouseroom.Isunderrenovation=0 THEN 'No' ELSE 'Yes' END AS Propertyhouseunderrenovation,
					CASE WHEN Systempropertyhouseroom.Isshop=0 THEN 'No' ELSE 'Yes' END AS Propertyhouseshop,
					CASE WHEN Systempropertyhouseroom.Isgroundfloor=0 THEN 'No' ELSE 'Yes' END AS Propertyhousegroundfloor,
					CASE WHEN Systempropertyhouseroom.Hasbalcony=0 THEN 'No' ELSE 'Yes' END AS Propertyhousebalcony,
					Systemhousekitchentype.Kitchentypename AS Propertyhousekitchentype
					FROM Systempropertyhouserooms Systempropertyhouseroom
					INNER JOIN Systemhousekitchentype Systemhousekitchentype ON Systempropertyhouseroom.Kitchentypeid=Systemhousekitchentype.Kitchentypeid
					INNER  JOIN Systempropertyhousesizes Systempropertyhousesize ON Systempropertyhouseroom.Systempropertyhousesizeid=Systempropertyhousesize.Systempropertyhousesizeid
					INNER JOIN Systemhousesizes Systemhousesize ON Systempropertyhousesize.Systemhousesizeid=Systemhousesize.Systemhousesizeid
					INNER JOIN  Systempropertyhouses Systempropertyhouse ON Systempropertyhousesize.Propertyhouseid=Systempropertyhouse.Propertyhouseid
					INNER JOIN Systemstaffs Systemstaff ON Systempropertyhouse.Propertyhouseowner=Systemstaff.Userid
					INNER JOIN Systemcounty Systemcounty ON Systempropertyhouse.Countyid=Systemcounty.Countyid
					INNER JOIN Systemsubcounty Systemsubcounty ON Systempropertyhouse.Subcountyid=Systemsubcounty.Subcountyid
					INNER JOIN Systemsubcountyward Systemsubcountyward ON Systempropertyhouse.Subcountywardid=Systemsubcountyward.Subcountywardid
					WHERE  Systempropertyhouseroom.Isvacant=1
					FOR JSON PATH
				 ) AS Data
				 FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
				 );

	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systempropertydata AS Systempropertydata;

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