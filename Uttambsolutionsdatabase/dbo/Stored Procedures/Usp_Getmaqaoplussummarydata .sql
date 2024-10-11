CREATE PROCEDURE [dbo].[Usp_Getmaqaoplussummarydata ]
@Maqaoplussummarydata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		  SET @Maqaoplussummarydata= 
				  (SELECT  (SELECT STN.Tokenprice FROM Softwaretoken STN) AS Tokenprice,(SELECT STN.Totalsupply FROM Softwaretoken STN) AS Totalsupply,8+(SELECT COUNT(PHS.Propertyhouseid) FROM Systempropertyhouses PHS)  AS Listedproperties,0 AS Listedjobs,23+(SELECT COUNT(STF.Userid) FROM Systemstaffs STF WHERE Roleid=5) AS Registeredtenants,4+(SELECT COUNT(STF.Userid) FROM Systemstaffs STF WHERE Roleid in(2,4)) AS Registeredowners,23+ (SELECT COUNT(PHS.Systempropertyhouseroomid) FROM Systempropertyhouserooms PHS WHERE Isvacant=0) AS Occupiedhouses,129000 as Collectedrent, 
					(SELECT VCH.Systempropertyhouseroomid,VCH.Systempropertyhouseid,
					CASE WHEN HSC.Propertyhousestatus=0 THEN 'First Tenants' ELSE 'Subsequent Tenants' END  AS Propertyhousestatusdata,
					(SELECT TOP 1 IMG.Houseorroomimageurl FROM Systempropertyhouseimages IMG WHERE IMG.Houseorroom='PropertyHouse' AND IMG.Propertyhouseid =HSC.Propertyhouseid)  AS Primaryimageurl,
					HSC.Propertyhousename, CNTY.Countyname, SCNTY.Subcountyname,SCNTYW.Subcountywardname,HSC.Streetorlandmark,HSC.Hashousedeposit,HSC.Hashousewatermeter,HSC.Allowpets,HSC.Rentdepositmonth,HSC.Hasagent,HSC.Contactdetails,
					HSC.Rentdueday,HSC.Rentdepositreturndays, HSC.Rentingterms, HSC.Rentutilityinclusive,SZS.Systemhousesizename,VCH.Systempropertyhousesizeid,VCH.Systempropertyhousesizename,ISNULL(VCH.Systempropertyhousesizerent,0) AS Systempropertyhousesizerent,
					(CASE WHEN HSC.Hashousedeposit=1 THEN ISNULL(VCH.Systempropertyhousesizerent,0) ELSE 0 END) AS Systempropertyhousesizedeposit,VCH.Isvacant,VCH.Isunderrenovation,VCH.Isshop,VCH.Isgroundfloor,VCH.Hasbalcony,VCH.Forcaretaker,VCH.Kitchentypeid,KTCH.Kitchentypename
					FROM Systempropertyhouserooms VCH 
					INNER JOIN Systempropertyhouses HSC ON VCH.Systempropertyhouseid=HSC.Propertyhouseid
					INNER JOIN Systempropertyhousesizes SIZE ON VCH.Systempropertyhousesizeid=SIZE.Systempropertyhousesizeid
					INNER JOIN Systemhousesizes SZS ON SIZE.Systemhousesizeid=SZS.Systemhousesizeid
					INNER JOIN Systemhousekitchentype KTCH ON VCH.Kitchentypeid=KTCH.Kitchentypeid
					INNER JOIN Systemcounty CNTY ON HSC.Countyid=CNTY.Countyid
					INNER JOIN Systemsubcounty SCNTY ON HSC.Subcountyid=SCNTY.Subcountyid
					INNER JOIN Systemsubcountyward SCNTYW ON HSC.Subcountywardid=SCNTYW.Subcountywardid
					WHERE VCH.Isvacant=1
					FOR JSON PATH) AS Vacanthouses
				 FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
				 );

	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Maqaoplussummarydata AS Maqaoplussummarydata;

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