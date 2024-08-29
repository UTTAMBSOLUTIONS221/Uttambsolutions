CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhouseroomdatabyid]
    @Houseroomid BIGINT,
    @Systempropertyhouseroomdata VARCHAR(MAX) OUTPUT
AS
BEGIN
    DECLARE 
        @RespStat INT = 0,
        @RespMsg VARCHAR(150) = 'Ok',
        @Systempropertyhousemetedata VARCHAR(MAX);

    BEGIN TRY
        BEGIN TRANSACTION;

        SET @Systempropertyhouseroomdata = (
		  CASE 
			WHEN EXISTS (SELECT 1 FROM Systempropertyhouseroommeters WHERE Systempropertyhouseroomid = @Houseroomid)
			THEN (
				SELECT (SELECT TOP 1 1 AS Hasprevious,
					Systempropertyhouseroom.Systempropertyhouseroomid,
					Systempropertyhouseroom.Systempropertyhouseid,
					Systempropertyhousesize.Systemhousesizeid AS Systempropertyhousesizeid,
					Systempropertyhouseroom.Systempropertyhousesizename,
					Systempropertyhouseroom.Systempropertyhousesizerent,
					Systempropertyhouseroom.Systempropertyhousesizedeposit,
					Systempropertyhouseroom.Isvacant,
					Systempropertyhouseroom.Isunderrenovation,
					Systempropertyhouseroom.Isshop,
					Systempropertyhouseroom.Isgroundfloor,
					Systempropertyhouseroom.Hasbalcony,
					Systempropertyhouseroom.Forcaretaker,
					Systempropertyhouseroom.Kitchentypeid,
					Systempropertyhouseroommeter.Systempropertyhousemeterid,
					Systempropertyhouseroommeter.Systempropertyhouseroommeternumber,
					ISNULL((SELECT TOP 1 Closingmeter FROM Systempropertyhouseroommeters WHERE Systempropertyhouseroomid = Systempropertyhouseroom.Systempropertyhouseroomid ORDER BY Datecreated DESC), 0.00) AS Openingmeter,
					0 AS Movedmeter,
					0 AS Closingmeter,
					ISNULL(TENANT.Systempropertyhousetenantid,0) AS Tenantid,
					ISNULL(TENANT.Roomoccupant,0) AS Roomoccupant,
					ISNULL(TENANT.Roomoccupantdetail,'')  AS Roomoccupantdetail,
					Systempropertyhouse.Waterunitprice,
					Systempropertyhouse.Hashousewatermeter,
					Systempropertyhouseroommeter.Createdby,
					Systempropertyhouseroommeter.Datecreated,
					Systemstaff.Firstname +' '+ Systemstaff.Lastname AS Fullname,Systemstaff.Phonenumber,Systemstaff.Emailaddress,
					(CASE WHEN Systemstaff.Genderid=0 THEN 'Not Set' WHEN Systemstaff.Genderid=1 THEN 'Male' WHEN Systemstaff.Genderid=2 THEN 'Female' ELSE 'Prefer not to Say' END) AS Gender,
					(CASE WHEN Systemstaff.Maritalstatusid=0 THEN 'Not Set' WHEN Systemstaff.Maritalstatusid=1 THEN 'Single' WHEN Systemstaff.Maritalstatusid=2 THEN 'Married' WHEN Systemstaff.Maritalstatusid=3 THEN 'Divorced' ELSE 'Prefer not to Say' END) AS Maritalstatus,
					Systemstaff.Loginstatus,Systemstaff.Parentid,Systemstaff.Userprofileimageurl,Systemstaff.Usercurriculumvitae,Systemstaff.Idnumber,Systemstaff.Updateprofile,Systemstaffsaccount.Accountnumber,Systemstaffsaccount.Accountid,0 AS Walletbalance,Systemstaff.Datemodified,
					(
						SELECT 
							Systempropertyhouseroommeterhist.Systempropertyhousemeterid,
							Systempropertyhouseroommeterhist.Systempropertyhouseroomid,
							Systempropertyhouseroommeterhist.Systempropertyhouseroommeternumber,
							Systempropertyhouseroommeterhist.Openingmeter,
							Systempropertyhouseroommeterhist.Movedmeter,
							Systempropertyhouseroommeterhist.Closingmeter,
							Systempropertyhouseroommeterhist.Consumedamount,
							Systempropertyhouseroommeterhist.Createdby,
							Systempropertyhouseroommeterhist.Datecreated
						FROM Systempropertyhouseroommeters Systempropertyhouseroommeterhist
						WHERE Systempropertyhouseroom.Systempropertyhouseroomid=Systempropertyhouseroommeterhist.Systempropertyhouseroomid
						FOR JSON PATH
					) AS Meterhistorydata,
					(SELECT Systemcounty.Countyname+' >> '+ Systemsubcounty.Subcountyname AS Countyname,Systemsubcounty.Subcountyname,Systemsubcountyward.Subcountywardname,Systempropertyhouse.Streetorlandmark,Systempropertyhouse.Propertyhousename,Systempropertyowner.Firstname+' '+Systempropertyowner.Lastname AS Propertyownername ,Systemhousesize.Systemhousesizename +'-'+ Systempropertyhouseroom.Systempropertyhousesizename AS Systempropertyhousesizename,
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
					INNER JOIN Systemstaffs Modifiedby ON Systempropertyhouseroomstenant.Modifiedby=Modifiedby.Userid 
					WHERE Systempropertyhouseroomstenant.Systempropertyhousetenantid=TENANT.Systempropertyhousetenantid
					ORDER BY Systempropertyhouseroomstenant.Datecreated ASC 
					FOR JSON PATH
					) AS Roomtenanthistorydata
				FROM Systempropertyhouserooms Systempropertyhouseroom
				INNER JOIN Systempropertyhouses Systempropertyhouse ON Systempropertyhouseroom.Systempropertyhouseid =Systempropertyhouse.Propertyhouseid
				INNER JOIN Systempropertyhousesizes Systempropertyhousesize ON Systempropertyhouseroom.Systempropertyhousesizeid=Systempropertyhousesize.Systempropertyhousesizeid
				INNER JOIN Systemhousesizes Systemhousesize ON Systempropertyhousesize.Systemhousesizeid=Systemhousesize.Systemhousesizeid
				LEFT JOIN Systempropertyhouseroomstenant TENANT ON Systempropertyhouseroom.Systempropertyhouseroomid=TENANT.Systempropertyhouseroomid
				LEFT JOIN Systemstaffs Systemstaff ON TENANT.Systempropertyhousetenantid=Systemstaff.Userid
				LEFT JOIN Systemstaffsaccount Systemstaffsaccount ON Systemstaff.Userid=Systemstaffsaccount.Userid
				LEFT JOIN Systempropertyhouseroommeters Systempropertyhouseroommeter ON Systempropertyhouseroom.Systempropertyhouseroomid=Systempropertyhouseroommeter.Systempropertyhouseroomid
                WHERE Systempropertyhouseroom.Systempropertyhouseroomid = @Houseroomid
                FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
                ) AS Data
				FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
				)
                ELSE (
                    SELECT(SELECT 
                        0 AS Hasprevious,
						Systempropertyhouseroom.Systempropertyhouseroomid,
						Systempropertyhouseroom.Systempropertyhouseid,
						Systempropertyhousesize.Systemhousesizeid AS Systempropertyhousesizeid,
						Systempropertyhouseroom.Systempropertyhousesizename,
						Systempropertyhouseroom.Systempropertyhousesizerent,
						Systempropertyhouseroom.Systempropertyhousesizedeposit,
						Systempropertyhouseroom.Isvacant,
						Systempropertyhouseroom.Isunderrenovation,
						Systempropertyhouseroom.Isshop,
						Systempropertyhouseroom.Isgroundfloor,
						Systempropertyhouseroom.Hasbalcony,
						Systempropertyhouseroom.Forcaretaker,
						Systempropertyhouseroom.Kitchentypeid,
                        0 AS Systempropertyhousemeterid,
                        'Meter123' AS Systempropertyhouseroommeternumber,
                        0.00 AS Openingmeter,
                        0.00 AS Movedmeter,
                        0.00 AS Closingmeter,
						0 AS Roomoccupant,
						0 AS Roomoccupantdetail,
						0 AS Tenantid,
						Systempropertyhouse.Hashousewatermeter,
						Systempropertyhouse.Waterunitprice,
                        0 AS Createdby,
						'' AS Fullname,'' AS Phonenumber,'' AS Emailaddress,'' AS Gender,'' AS Maritalstatus,
					0 AS Loginstatus,0 AS Parentid,'' AS Userprofileimageurl,'' AS Usercurriculumvitae,0 AS Idnumber,0 AS Updateprofile,0 AS Accountnumber,0 AS Accountid,0 AS Walletbalance,GETDATE() AS Datemodified,
					
                        GETDATE() AS Datecreated,
                        NULL AS Meterhistorydata,
						NULL AS Roomtenanthistorydata
				FROM Systempropertyhouserooms Systempropertyhouseroom
				INNER JOIN Systempropertyhouses Systempropertyhouse ON Systempropertyhouseroom.Systempropertyhouseid =Systempropertyhouse.Propertyhouseid
				INNER JOIN Systempropertyhousesizes Systempropertyhousesize ON Systempropertyhouseroom.Systempropertyhousesizeid=Systempropertyhousesize.Systempropertyhousesizeid
				INNER JOIN Systemhousesizes Systemhousesize ON Systempropertyhousesize.Systemhousesizeid=Systemhousesize.Systemhousesizeid
				LEFT JOIN Systempropertyhouseroommeters Systempropertyhouseroommeter ON Systempropertyhouseroom.Systempropertyhouseroomid=Systempropertyhouseroommeter.Systempropertyhouseroomid
                WHERE Systempropertyhouseroom.Systempropertyhouseroomid = @Houseroomid
				FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
              ) AS Data
			FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
			)
            END
        );

        SET @RespMsg = 'Ok.';
        SET @RespStat = 0;
        COMMIT TRANSACTION;

        SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage,@Systempropertyhouseroomdata AS Systempropertyhouseroomdata;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT 'Error ' + ERROR_MESSAGE();
        SELECT 2 AS RespStatus, '0 - Error(s) Occurred: ' + ERROR_MESSAGE() AS RespMessage;
    END CATCH

    RETURN; 
END