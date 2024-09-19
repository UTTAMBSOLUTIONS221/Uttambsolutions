CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhousedatabyownerid]
@Ownerid BIGINT,
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
				  (SELECT (SELECT Systempropertyhouse.Propertyhouseid,Systempropertyhouse.Isagency,Systempropertyhouse.Propertyhouseowner,Systemstaff.Firstname+''+Systemstaff.Lastname AS Propertyhouseownername,Systempropertyhouse.Propertyhouseposter,Systempropertyhouse.Propertyhousename,
					(SELECT TOP 1  IMG.Houseorroomimageurl FROM Systempropertyhouseimages IMG WHERE IMG.Houseorroom= 'PropertyHouse' AND IMG.Propertyhouseid=Systempropertyhouse.Propertyhouseid ORDER BY IMG.Datecreated )AS Primaryimageurl,
					Systempropertyhouse.Countyid,Systemcounty.Countyname,Systempropertyhouse.Subcountyid,Systemsubcounty.Subcountyname,Systempropertyhouse.Subcountywardid,Systemsubcountyward.Subcountywardname,Systempropertyhouse.Streetorlandmark,Systempropertyhouse.Hashousedeposit,
					CASE WHEN Systempropertyhouse.Propertyhousestatus=0 THEN 'First Tenants' ELSE 'Subsequent Tenants' END  AS Propertyhousestatusdata,Systempropertyhouse.Propertyhousestatus,Systempropertyhouse.Monthlycollection,Systempropertyhouse.Extra,Systempropertyhouse.Extra1,Systempropertyhouse.Extra2,Systempropertyhouse.Extra3,Systempropertyhouse.Extra4,
					Systempropertyhouse.Extra5,Systempropertyhouse.Extra6,Systempropertyhouse.Extra7,Systempropertyhouse.Extra8,Systempropertyhouse.Extra9,Systempropertyhouse.Extra10,Systempropertyhouse.Createdby,Systempropertyhouse.Modifiedby,Systempropertyhouse.Datecreated,Systempropertyhouse.Datemodified,
					(SELECT SUM(Systempropertyhousesize.Systempropertyhousesizeunits) FROM Systempropertyhousesizes Systempropertyhousesize WHERE Systempropertyhousesize.Propertyhouseid=Systempropertyhouse.Propertyhouseid) AS Roomscount,
					(
						SELECT 
							ISNULL(Systempropertyhousesize.Systempropertyhousesizeid, 0) AS Systempropertyhousesizeid,
							ISNULL(Systempropertyhousesize.Propertyhouseid, 0) AS Propertyhouseid,
							ISNULL(Systemhousesize.Systemhousesizeid, 0) AS Systemhousesizeid,
							ISNULL(Systemhousesize.Systemhousesizename, '') AS Systemhousesizename,
							ISNULL(Systempropertyhousesize.Systempropertyhousesizeunits, 0) AS Systempropertyhousesizeunits,
							ISNULL(Systempropertyhouseroom.Systempropertyhousesizerent, 0) AS Systempropertyhousesizerent,
							ISNULL(Systempropertyhouseroom.Systempropertyhousesizedeposit, 0) AS Systempropertyhousesizedeposit,
							ISNULL(Systempropertyhousesize.Systempropertyhousesizewehave, 0) AS Systempropertyhousesizewehave
						FROM Systemhousesizes Systemhousesize
						INNER JOIN Systempropertyhouserooms Systempropertyhouseroom ON Systemhousesize.Systemhousesizeid=Systempropertyhouseroom.Systempropertyhousesizeid
						LEFT JOIN Systempropertyhousesizes Systempropertyhousesize ON Systemhousesize.Systemhousesizeid = Systempropertyhousesize.Systemhousesizeid
						WHERE Systempropertyhouse.Propertyhouseid=Systempropertyhousesize.Propertyhouseid
						FOR JSON PATH
					) AS Propertyhousesize,
					(
						SELECT 
							ISNULL(Systempropertyhousedepositfee.Systempropertyhousedepositfeeid, 0) AS Systempropertyhousedepositfeeid,
							ISNULL(Systempropertyhousedepositfee.Propertyhouseid, 0) AS Propertyhouseid,
							ISNULL(Systemhousedepositfee.Housedepositfeeid, 0) AS Housedepositfeeid,
							ISNULL(Systemhousedepositfee.Housedepositfeename, '') AS Housedepositfeename,
							ISNULL(Systempropertyhousedepositfee.Systempropertyhousedepositfeeamount, 0) AS Systempropertyhousedepositfeeamount,
							ISNULL(Systempropertyhousedepositfee.Systempropertyhousesizedepositfeewehave, 0) AS Systempropertyhousesizedepositfeewehave
						FROM Systemhousedepositfees Systemhousedepositfee
						LEFT JOIN Systempropertyhousedepositfees Systempropertyhousedepositfee ON Systemhousedepositfee.Housedepositfeeid = Systempropertyhousedepositfee.Housedepositfeeid
						WHERE Systempropertyhouse.Propertyhouseid=Systempropertyhousedepositfee.Propertyhouseid
						FOR JSON PATH
					) AS Propertyhousedepositfee,
					(
						SELECT 
							ISNULL(Systempropertyhousebenefit.Systempropertyhousebenefitid, 0) AS Systempropertyhousebenefitid,
							ISNULL(Systempropertyhousebenefit.Propertyhouseid, 0) AS Propertyhouseid,
							ISNULL(Systemhousebenefit.Housebenefitid, 0) AS Housebenefitid,
							ISNULL(Systemhousebenefit.Housebenefitname, '') AS Housebenefitname,
							ISNULL(Systempropertyhousebenefit.Systempropertyhousebenefitwehave, 0) AS Systempropertyhousebenefitwehave
						FROM Systemhousebenefits Systemhousebenefit
						LEFT JOIN Systempropertyhousebenefits Systempropertyhousebenefit ON Systemhousebenefit.Housebenefitid = Systempropertyhousebenefit.Housebenefitid
						WHERE Systempropertyhouse.Propertyhouseid=Systempropertyhousebenefit.Propertyhouseid
						FOR JSON PATH
					) AS Propertyhousebenefit
					FROM Systempropertyhouses Systempropertyhouse
					INNER JOIN Systemstaffs Systemstaff ON Systempropertyhouse.Propertyhouseowner=Systemstaff.Userid
					INNER JOIN Systemcounty Systemcounty ON Systempropertyhouse.Countyid=Systemcounty.Countyid
					INNER JOIN Systemsubcounty Systemsubcounty ON Systempropertyhouse.Subcountyid=Systemsubcounty.Subcountyid
					INNER JOIN Systemsubcountyward Systemsubcountyward ON Systempropertyhouse.Subcountywardid=Systemsubcountyward.Subcountywardid
					WHERE Systempropertyhouse.Propertyhouseowner=@Ownerid AND Systempropertyhouse.Propertyhouseposter=@Ownerid
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