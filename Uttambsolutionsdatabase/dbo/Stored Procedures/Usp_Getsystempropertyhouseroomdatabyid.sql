
CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhouseroomdatabyid]
    @Houseroomid BIGINT,
    @Systempropertyhouseroomdata VARCHAR(MAX) OUTPUT
AS
BEGIN
    DECLARE 
        @RespStat INT = 0,
        @RespMsg VARCHAR(150) = 'Ok';

    BEGIN TRY
        BEGIN TRANSACTION;

        SET @Systempropertyhouseroomdata = (
            SELECT(SELECT 
                CASE 
                    WHEN EXISTS (SELECT 1 FROM Systempropertyhouseroommeters WHERE Systempropertyhouseroomid = @Houseroomid) 
                    THEN 1 
                    ELSE 0 
                END AS Hasprevious,
                spr.Systempropertyhouseroomid,
                spr.Systempropertyhouseid,
                sph.Systemhousesizeid AS Systempropertyhousesizeid,
                spr.Systempropertyhousesizename,
                spr.Systempropertyhousesizerent,
                spr.Systempropertyhousesizedeposit,
                spr.Isvacant,
                spr.Isunderrenovation,
                spr.Isshop,
                spr.Isgroundfloor,
                spr.Hasbalcony,
                spr.Forcaretaker,
                spr.Kitchentypeid,
                ISNULL(sprm.Systempropertyhousemeterid,0) AS Systempropertyhousemeterid,
                ISNULL(sprm.Systempropertyhouseroommeternumber,'Meter' +spr.Systempropertyhousesizename) AS Systempropertyhouseroommeternumber,
                ISNULL((SELECT TOP 1 Closingmeter 
                        FROM Systempropertyhouseroommeters 
                        WHERE Systempropertyhouseroomid = spr.Systempropertyhouseroomid 
                        ORDER BY Datecreated DESC), 0.00) AS Openingmeter,
                0 AS Movedmeter,
                0 AS Closingmeter,
                ISNULL(t.Systempropertyhousetenantid, 0) AS Tenantid,
                ISNULL(t.Roomoccupant, 0) AS Roomoccupant,
                ISNULL(t.Roomoccupantdetail, '') AS Roomoccupantdetail,
                shp.Waterunitprice,
                shp.Hashousewatermeter,
                ISNULL(sprm.Createdby,0) AS Createdby,
                ISNULL(sprm.Datecreated,GETDATE()) AS Datecreated,
                st.Firstname + ' ' + st.Lastname AS Fullname,
                st.Phonenumber,
                st.Emailaddress,
                CASE 
                    WHEN st.Genderid = 0 THEN 'Not Set' 
                    WHEN st.Genderid = 1 THEN 'Male' 
                    WHEN st.Genderid = 2 THEN 'Female' 
                    ELSE 'Prefer not to Say' 
                END AS Gender,
                CASE 
                    WHEN st.Maritalstatusid = 0 THEN 'Not Set' 
                    WHEN st.Maritalstatusid = 1 THEN 'Single' 
                    WHEN st.Maritalstatusid = 2 THEN 'Married' 
                    WHEN st.Maritalstatusid = 3 THEN 'Divorced' 
                    ELSE 'Prefer not to Say' 
                END AS Maritalstatus,
                ISNULL(st.Loginstatus,0) AS Loginstatus,
                ISNULL(st.Parentid,0) AS Parentid,
                st.Userprofileimageurl,
                st.Usercurriculumvitae,
                ISNULL(st.Idnumber,0) AS Idnumber,
                ISNULL(st.Updateprofile,0) AS Updateprofile,
                ISNULL(sa.Accountnumber,0) AS Accountnumber,
                ISNULL(sa.Accountid,0) AS Accountid,
                0 AS Walletbalance,
                ISNULL(st.Datemodified,GETDATE()) AS Datemodified,
                (
                    SELECT 
                        sprm.Systempropertyhousemeterid,
                        sprm.Systempropertyhouseroomid,
                        sprm.Systempropertyhouseroommeternumber,
                        sprm.Openingmeter,
                        sprm.Movedmeter,
                        sprm.Closingmeter,
                        sprm.Consumedamount,
                        sprm.Createdby,
                        sprm.Datecreated
                    FROM Systempropertyhouseroommeters sprm
                    WHERE spr.Systempropertyhouseroomid = sprm.Systempropertyhouseroomid
                    FOR JSON PATH
                ) AS Meterhistorydata,
				(CASE 
                    WHEN EXISTS (SELECT 1 FROM Systempropertyhousechecklists WHERE Propertyhouseroomid = spr.Systempropertyhouseroomid)
                    THEN 
                    (SELECT ISNULL(CQL.Propertychecklistid, 0) AS Propertychecklistid,
                            ISNULL(CQL.Propertyhouseroomid, 0) AS Propertyhouseroomid,
                            ISNULL(CQL.Fixturestatusid, 0) AS Fixturestatusid,
                            ISNULL(CQL.Fixtureunits, 0) AS Fixtureunits,
                            ISNULL(STS.Fixturestatus, 'Not Set') AS Fixturestatus,
                            ISNULL(CQL.Createdby, shp.Propertyhouseposter) AS Createdby,
                            ISNULL(CQL.Datecreated, GETDATE()) AS Datecreated,
                            FIX.Fixtureid,
                            FIX.Fixturetype,
                            FIX.Descriptions,
                            FIX.Category
                    FROM Systemfixtures FIX
                    LEFT JOIN Systempropertyhousechecklists CQL ON CQL.Fixtureid = FIX.Fixtureid
                    LEFT JOIN Systemfixturestatus STS ON CQL.Fixturestatusid = STS.Fixturestatusid
                    WHERE CQL.Propertyhouseroomid = spr.Systempropertyhouseroomid
                    FOR JSON PATH
                    )
                    ELSE 
                    (SELECT 0 AS Propertychecklistid,
                            0 AS Fixturestatusid,
                            0 AS Fixtureunits,
                            '' AS Fixturestatus,
                            FIX.Fixtureid,
                            FIX.Fixturetype,
                            FIX.Descriptions,
                            FIX.Category
                    FROM Systemfixtures FIX
                    FOR JSON PATH
                    )
                END) AS Roomfixtures,
                (
                    SELECT 
                        sc.Countyname AS Countyname,
                        ss.Subcountyname,
                        ssw.Subcountywardname,
                        shp.Streetorlandmark,
                        shp.Propertyhousename,
                        spo.Firstname + ' ' + spo.Lastname AS Propertyownername,
                        sh.Systemhousesizename + '-' + spr.Systempropertyhousesizename AS Systempropertyhousesizename,
                        0 AS Outstandingbalance,
                        cb.Firstname + ' ' + cb.Lastname AS Createdby,
                        mb.Firstname + ' ' + mb.Lastname AS Modifiedby,
                        sprt.Datemodified
                    FROM Systempropertyhouseroomstenant sprt
                    INNER JOIN Systempropertyhouserooms spr ON spr.Systempropertyhouseroomid = sprt.Systempropertyhouseroomid
                    INNER JOIN Systempropertyhousesizes sph ON spr.Systempropertyhousesizeid = sph.Systempropertyhousesizeid
                    INNER JOIN Systemhousesizes sh ON sph.Systemhousesizeid = sh.Systemhousesizeid
                    INNER JOIN Systempropertyhouses shp ON spr.Systempropertyhouseid = shp.Propertyhouseid
                    INNER JOIN Systemcounty sc ON shp.Countyid = sc.CountyId
                    INNER JOIN Systemsubcounty ss ON shp.SubcountyId = ss.SubcountyId
                    INNER JOIN Systemsubcountyward ssw ON shp.Subcountywardid = ssw.Subcountywardid
                    INNER JOIN Systemstaffs spo ON shp.Propertyhouseowner = spo.Userid
                    INNER JOIN Systemstaffs cb ON sprt.Createdby = cb.Userid
                    INNER JOIN Systemstaffs mb ON sprt.Modifiedby = mb.Userid
                    WHERE sprt.Systempropertyhousetenantid = t.Systempropertyhousetenantid
                    ORDER BY sprt.Datecreated ASC 
                    FOR JSON PATH
                ) AS Roomtenanthistorydata
            FROM Systempropertyhouserooms spr
            INNER JOIN Systempropertyhouses shp ON spr.Systempropertyhouseid = shp.Propertyhouseid
            INNER JOIN Systempropertyhousesizes sph ON spr.Systempropertyhousesizeid = sph.Systempropertyhousesizeid
            INNER JOIN Systemhousesizes sh ON sph.Systemhousesizeid = sh.Systemhousesizeid
            LEFT JOIN Systempropertyhouseroomstenant t ON spr.Systempropertyhouseroomid = t.Systempropertyhouseroomid
            LEFT JOIN Systemstaffs st ON t.Systempropertyhousetenantid = st.Userid
            LEFT JOIN Systemstaffsaccount sa ON st.Userid = sa.Userid
            LEFT JOIN Systempropertyhouseroommeters sprm ON spr.Systempropertyhouseroomid = sprm.Systempropertyhouseroomid
            WHERE spr.Systempropertyhouseroomid = @Houseroomid
            FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
			) AS Data
			FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @RespStat = ERROR_NUMBER();
        SET @RespMsg = ERROR_MESSAGE();
        ROLLBACK TRANSACTION;
    END CATCH

    SELECT @RespStat AS Status, @RespMsg AS Message;
END