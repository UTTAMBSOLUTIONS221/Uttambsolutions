CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhousedata]
AS
BEGIN
   BEGIN
	DECLARE @RespStat int = 0,
			@RespMsg varchar(150) = '';
		  
	BEGIN
		BEGIN TRY	
		--Validate

		BEGIN TRANSACTION;
		    SELECT Systempropertyhouse.Propertyhouseid,Systempropertyhouse.Isagency,Systempropertyhouse.Propertyhouseowner,Systemstaff.Firstname+''+Systemstaff.Lastname AS Propertyhouseownername,Systempropertyhouse.Propertyhouseposter,Systempropertyhouse.Propertyhousename,
			Systempropertyhouse.Countyid,Systemcounty.Countyname,Systempropertyhouse.Subcountyid,Systemsubcounty.Subcountyname,Systempropertyhouse.Subcountywardid,Systemsubcountyward.Subcountywardname,Systempropertyhouse.Streetorlandmark,Systempropertyhouse.Hashousedeposit,
			CASE WHEN Systempropertyhouse.Propertyhousestatus=0 THEN 'First Tenants' ELSE 'Subsequent Tenants' END  AS Propertyhousestatusdata,Systempropertyhouse.Propertyhousestatus,Systempropertyhouse.Monthlycollection,Systempropertyhouse.Extra,Systempropertyhouse.Extra1,Systempropertyhouse.Extra2,Systempropertyhouse.Extra3,Systempropertyhouse.Extra4,
			Systempropertyhouse.Extra5,Systempropertyhouse.Extra6,Systempropertyhouse.Extra7,Systempropertyhouse.Extra8,Systempropertyhouse.Extra9,Systempropertyhouse.Extra10,Systempropertyhouse.Createdby,Systempropertyhouse.Modifiedby,Systempropertyhouse.Datecreated,Systempropertyhouse.Datemodified,
			(SELECT SUM(Systempropertyhousesize.Systempropertyhousesizeunits) FROM Systempropertyhousesizes Systempropertyhousesize WHERE Systempropertyhousesize.Propertyhouseid=Systempropertyhouse.Propertyhouseid) AS Roomscount,
			(SELECT TOP 1 IMG.Houseorroomimageurl FROM Systempropertyhouseimages IMG WHERE IMG.Houseorroom='PropertyHouse' AND IMG.Propertyhouseid =Systempropertyhouse.Propertyhouseid)  AS Primaryimageurl
			FROM Systempropertyhouses Systempropertyhouse
			INNER JOIN Systemstaffs Systemstaff ON Systempropertyhouse.Propertyhouseowner=Systemstaff.Userid
			INNER JOIN Systemcounty Systemcounty ON Systempropertyhouse.Countyid=Systemcounty.Countyid
			INNER JOIN Systemsubcounty Systemsubcounty ON Systempropertyhouse.Subcountyid=Systemsubcounty.Subcountyid
			INNER JOIN Systemsubcountyward Systemsubcountyward ON Systempropertyhouse.Subcountywardid=Systemsubcountyward.Subcountywardid
		Set @RespMsg ='Success'
		Set @RespStat =0; 
		COMMIT TRANSACTION;
		Select @RespStat as RespStatus, @RespMsg as RespMessage;
		SELECT * FROM Systempropertyhouseimages
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