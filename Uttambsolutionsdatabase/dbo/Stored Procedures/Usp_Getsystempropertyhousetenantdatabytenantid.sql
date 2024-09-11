CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhousetenantdatabytenantid]
    @Tenantid BIGINT,
	@SystemPropertyHouseTenantData VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		--validate	

		BEGIN TRANSACTION;
		  SET @SystemPropertyHouseTenantData = 
		    (SELECT(SELECT Systemstaff.Userid,Systemstaff.Firstname +' '+ Systemstaff.Lastname AS Fullname,Systemstaff.Phonenumber,Systemstaff.Emailaddress,
			(CASE WHEN Systemstaff.Genderid=0 THEN 'Not Set' WHEN Systemstaff.Genderid=1 THEN 'Male' WHEN Systemstaff.Genderid=2 THEN 'Female' ELSE 'Prefer not to Say' END) AS Gender,
			(CASE WHEN Systemstaff.Maritalstatusid=0 THEN 'Not Set' WHEN Systemstaff.Maritalstatusid=1 THEN 'Single' WHEN Systemstaff.Maritalstatusid=2 THEN 'Married' WHEN Systemstaff.Maritalstatusid=3 THEN 'Divorced' ELSE 'Prefer not to Say' END) AS Maritalstatus,
			Systemstaff.Loginstatus,Systemstaff.Parentid,Systemstaff.Userprofileimageurl,Systemstaff.Usercurriculumvitae,Systemstaff.Idnumber,Systemstaff.Updateprofile,Systemstaffsaccount.Accountnumber,Systemstaffsaccount.Accountid,0 AS Walletbalance,Systemstaff.Datemodified,Systemstaff.Datecreated,
			(SELECT Systempropertyhouseroomstenant.Systempropertyhousetenantentryid,Systempropertyhouseroom.Systempropertyhouseroomid,Systempropertyhouse.Propertyhousename,CASE WHEN Systempropertyhouseroomstenant.Isoccupant=1 THEN 'Occupant' ELSE 'Vacated' END AS Houseoccupationstatus, CASE WHEN Systempropertyhouseroomstenant.Occupationalstatus=0 THEN 'Vacated' WHEN Systempropertyhouseroomstenant.Occupationalstatus= 1 THEN 'Vacating' ELSE 'Occupant' END AS Occupationalstatus,
			0 AS Systempropertyhousesizerentdeposit,Systempropertyhouseroom.Systempropertyhousesizerent,(SELECT FEE.Systempropertyhousedepositfeeamount FROM Systempropertyhousedepositfees FEE INNER JOIN Systemhousedepositfees FE ON FEE.Housedepositfeeid=FE.Housedepositfeeid WHERE FE.Housedepositfeename='Bin Fee' AND FEE.Propertyhouseid= Systempropertyhouse.Propertyhouseid) AS Monthlybinfee,Systempropertyhouse.Waterunitprice,0 AS Previousmeters,0 AS Previousmeteramount,Systempropertyhouse.Vacantnoticeperiod AS Vacatingperioddays,
			Systemhousesize.Systemhousesizename+'-'+ Systempropertyhouseroom.Systempropertyhousesizename AS Systempropertyhousesizename,Systempropertyhouseroomstenant.Datecreated,Systempropertyhouseroomstenant.Datemodified,
			(SELECT Systempropertyhouseroommeter.Systempropertyhouseroommeternumber,Systempropertyhouseroommeter.Openingmeter,Systempropertyhouseroommeter.Movedmeter,Systempropertyhouseroommeter.Closingmeter,Systempropertyhouseroommeter.Consumedamount,
			Createdby.Firstname+' '+Createdby.Lastname AS Createdby,  CAST(Systempropertyhouseroommeter.Datecreated AS date) AS Datecreated
			FROM Systempropertyhouseroommeters Systempropertyhouseroommeter
			INNER JOIN Systemstaffs Createdby ON Systempropertyhouseroommeter.Createdby=Createdby.Userid
			WHERE Systempropertyhouseroomstenant.Systempropertyhouseroomid=Systempropertyhouseroommeter.Systempropertyhouseroomid
			FOR JSON PATH ) AS Tenantroommeter
			FROM Systempropertyhouseroomstenant Systempropertyhouseroomstenant
			INNER JOIN Systempropertyhouserooms Systempropertyhouseroom  ON Systempropertyhouseroomstenant.Systempropertyhouseroomid=Systempropertyhouseroom.Systempropertyhouseroomid
			INNER JOIN Systempropertyhousesizes Systempropertyhousesize ON Systempropertyhouseroom.Systempropertyhousesizeid=Systempropertyhousesize.Systempropertyhousesizeid
			INNER JOIN Systemhousesizes Systemhousesize ON Systempropertyhousesize.Systemhousesizeid=Systemhousesize.Systemhousesizeid
			INNER JOIN Systempropertyhouses Systempropertyhouse ON Systempropertyhousesize.Propertyhouseid=Systempropertyhouse.Propertyhouseid
			WHERE Systempropertyhouseroomstenant.Isoccupant=1 AND Systempropertyhouseroomstenant.Systempropertyhousetenantid=Systemstaff.Userid
			FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
			)AS Tenantroomdata,
			(SELECT Systemcounty.Countyname AS Countyname,Systemsubcounty.Subcountyname,Systemsubcountyward.Subcountywardname,Systempropertyhouse.Streetorlandmark,Systempropertyhouse.Propertyhousename,Systempropertyowner.Firstname+' '+Systempropertyowner.Lastname AS Propertyownername ,Systemhousesize.Systemhousesizename +'-'+ Systempropertyhouseroom.Systempropertyhousesizename AS Systempropertyhousesizename,
			0 AS Outstandingbalance,Createdby.Firstname+' '+Createdby.Lastname AS Createdby,Modifiedby.Firstname+' '+Modifiedby.Lastname AS Modifiedby,Systempropertyhouseroomstenant.Datemodified
			FROM Systempropertyhouseroomstenant Systempropertyhouseroomstenant
			INNER JOIN Systempropertyhouserooms Systempropertyhouseroom ON Systempropertyhouseroom.Systempropertyhouseroomid=Systempropertyhouseroomstenant.Systempropertyhouseroomid
		    INNER JOIN Systempropertyhousesizes Systempropertyhousesize ON Systempropertyhouseroom.Systempropertyhousesizeid=Systempropertyhousesize.Systempropertyhousesizeid
			INNER JOIN Systemhousesizes Systemhousesize ON Systempropertyhousesize.Systemhousesizeid=Systemhousesize.Systemhousesizeid
			INNER JOIN Systempropertyhouses Systempropertyhouse ON Systempropertyhouseroom.Systempropertyhouseid=Systempropertyhouse.Propertyhouseid
			INNER JOIN Systemcounty Systemcounty ON Systempropertyhouse.Countyid=Systemcounty.CountyId
			INNER JOIN Systemsubcounty Systemsubcounty  ON Systempropertyhouse.SubcountyId=Systemsubcounty.SubcountyId
			INNER JOIN Systemsubcountyward Systemsubcountyward  ON Systempropertyhouse.Subcountywardid=Systemsubcountyward.Subcountywardid
			INNER JOIN Systemstaffs Systempropertyowner ON Systempropertyhouse.Propertyhouseowner=Systempropertyowner.Userid
			INNER JOIN Systemstaffs Createdby ON Systempropertyhouseroomstenant.Createdby=Createdby.Userid
			INNER JOIN Systemstaffs Modifiedby ON Systempropertyhouseroomstenant.Modifiedby=Modifiedby.Userid WHERE Systempropertyhouseroomstenant.Systempropertyhousetenantid=Systemstaff.Userid
			ORDER BY Systempropertyhouseroomstenant.Datecreated ASC 
			FOR JSON PATH
			) AS Tenantroomhistory
			FROM Systemstaffs Systemstaff
			INNER JOIN Systemstaffsaccount Systemstaffsaccount ON  Systemstaff.Userid=Systemstaffsaccount.Userid
			WHERE Systemstaff.Userid=@Tenantid
			FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			) AS Data
			FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER);
	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;
           SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@SystemPropertyHouseTenantData AS SystemPropertyHouseTenantData  
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