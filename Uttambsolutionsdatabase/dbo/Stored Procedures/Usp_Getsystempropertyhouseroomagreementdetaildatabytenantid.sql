CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhouseroomagreementdetaildatabytenantid]
@Propertytenantid BIGINT,
@TenantAgreementDetailData VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		SET @TenantAgreementDetailData = (SELECT (SELECT STAFF.Userid,STAFF.Firstname+' '+STAFF.Lastname AS Tenantfullname,STAFF.Phonenumber AS Tenantphonenumber,STAFF.Emailaddress AS Tenantemailaddress,STAFF.Idnumber AS Tenantidnumber,
				OWN.Firstname+' '+OWN.Lastname AS Ownerfullname,OWN.Phonenumber AS Ownerphonenumber,OWN.Emailaddress AS Owneremailaddress,ISNULL(OWN.Idnumber,0) AS Owneridnumber,HOUSE.Propertyhousename,HOUSE.Rentdueday,HOUSE.Vacantnoticeperiod,HOUSE.Hasagent,HOUSE.Hashousewatermeter,WATER.Systemhousewatertypename,
				SIZE.Systemhousesizename + ' '+ ROOM.Systempropertyhousesizename AS Systempropertyhousesizename,ROOM.Systempropertyhousesizerent,CASE WHEN HOUSE.Hashousedeposit =1 THEN  (ROOM.Systempropertyhousesizerent * HOUSE.Rentdepositmonth) ELSE 0 END AS Systempropertyhousesizerentdeposit,HOUSE.Rentdepositmonth,10 AS Rentdepositrefunddays,DATEADD(DAY, HOUSE.Rentdueday, DATEADD(MONTH, 1, GETDATE())) AS Nextrentduedate,1 AS Monthlyrentterms,0 AS Allowpets,HOUSE.Waterunitprice,
				Systemcounty.Countyname,Systemsubcounty.Subcountyname,Systemsubcountyward.Subcountywardname,HOUSE.Streetorlandmark,ISNULL(SPRA.Datecreated,GETDATE()) AS TenantDatecreated,ISNULL(SPRA.Signatureimageurl,'') AS TenantSignatureimageurl, (SELECT ISNULL(AGR.Signatureimageurl,'') FROM Systempropertyhouseagreements AGR WHERE AGR.Propertyhouseid = ROOM.Systempropertyhouseid) AS OwnerSignatureimageurl,'' AS Agreementdata,
				(SELECT STRING_AGG(FEE.Housedepositfeename + ' ' + CONVERT(VARCHAR(40), UTILITY.Systempropertyhousedepositfeeamount), ', ') AS CombinedFees FROM Systempropertyhousedepositfees UTILITY INNER JOIN Systemhousedepositfees FEE ON UTILITY.Housedepositfeeid = FEE.Housedepositfeeid WHERE UTILITY.Systempropertyhousesizedepositfeewehave = 1 AND UTILITY.Propertyhouseid=HOUSE.Propertyhouseid) AS Propertyhouseutility,
				(SELECT STRING_AGG(BNK.Systembankname+ ' - ' + CONVERT(VARCHAR(10),BNK.Systembankpaybill)  + ' - ' + CONVERT(VARCHAR(40), ACC.Systempropertybankaccount), ', ') AS CombinedFees FROM Systempropertybankaccounts ACC INNER JOIN Systemsupportedbanks BNK ON ACC.Systembankid = BNK.Systembankid WHERE ACC.Systempropertyhousebankwehave = 1 AND ACC.Propertyhouseid=HOUSE.Propertyhouseid) AS Systempropertybankname,
				'' AS Tenantsintheroom,0 AS Rentutilityinclusive
				FROM Systemstaffs STAFF
				INNER JOIN Systemstaffdesignations DESG ON STAFF.Userid=DESG.Systemstaffid
				LEFT JOIN Systempropertyhouseroomstenant TENANT  ON STAFF.Userid=TENANT.Systempropertyhousetenantid
				LEFT JOIN Systempropertyhouserooms ROOM ON TENANT.Systempropertyhouseroomid=ROOM.Systempropertyhouseroomid
				LEFT JOIN Systempropertyhousesizes ROOMSIZE ON ROOM.Systempropertyhousesizeid=ROOMSIZE.Systempropertyhousesizeid
				LEFT JOIN Systemhousesizes SIZE ON ROOMSIZE.Systemhousesizeid=SIZE.Systemhousesizeid
				LEFT JOIN Systempropertyhouses HOUSE ON ROOM.Systempropertyhouseid = HOUSE.Propertyhouseid
				LEFT JOIN Systemstaffs OWN ON HOUSE.Propertyhouseowner =OWN.Userid
				LEFT JOIN Systemcounty Systemcounty ON HOUSE.Countyid=Systemcounty.Countyid
				LEFT JOIN Systemsubcounty Systemsubcounty ON HOUSE.Subcountyid=Systemsubcounty.Subcountyid
				LEFT JOIN Systemsubcountyward Systemsubcountyward ON HOUSE.Subcountywardid=Systemsubcountyward.Subcountywardid
				LEFT JOIN Systempropertyhouseagreements SPRA ON STAFF.Userid=SPRA.Ownerortenantid
				LEFT JOIN Systemhousewatertype WATER ON HOUSE.Watertypeid=WATER.Systemhousewatertypeid
				WHERE DESG.Staffdesignation='Tenant' AND TENANT.Isoccupant=1 AND STAFF.Userid=@Propertytenantid
			FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			) AS Data
			FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			);
		Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@TenantAgreementDetailData AS TenantAgreementDetailData
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