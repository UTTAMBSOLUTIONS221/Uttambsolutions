CREATE PROCEDURE [dbo].[Usp_Registersystemagentpropertyhousedata]
@JsonObjectdata VARCHAR(MAX)
AS
BEGIN
   BEGIN
	DECLARE @RespStat int = 0,
			@RespMsg varchar(150) = '',
			@UserId BIGINT,
            @AccountId INT,
            @AccountNumber INT,
			@Propertyhouseid BIGINT = NULL;
		  
	BEGIN
		BEGIN TRY	
		--Validate

		BEGIN TRANSACTION;
		DECLARE @Systempropertyhousedata TABLE (Propertyhouseid BIGINT);
		 DECLARE @Systemstaffdata TABLE (Action VARCHAR(100), UserId BIGINT, FullName VARCHAR(140), Passwords VARCHAR(100), PassHarsh VARCHAR(100), UserName VARCHAR(100), EmailAddress VARCHAR(100));

       

		DECLARE @TempData TABLE (Propertyhouseid BIGINT,Isagency BIT,Propertyhouseowner BIGINT,Propertyhouseposter BIGINT,Propertyhousename VARCHAR(255),Countyid INT,Subcountyid INT,Subcountywardid INT,Streetorlandmark VARCHAR(255),Contactdetails VARCHAR(255),Hashousedeposit BIT,
		Rentdepositmonth INT,Hasagent BIT,Propertyhousestatus VARCHAR(50),Watertypeid BIGINT,Hashousewatermeter BIT,Waterunitprice DECIMAL(10, 2),Rentdueday INT,Vacantnoticeperiod INT,Monthlycollection DECIMAL(18,2),Rentutilityinclusive BIT,Rentdepositreturndays INT,Allowpets BIT,Rentingterms VARCHAR(20),Enddate DATETIME2,Numberofpets INT,Petdeposit DECIMAL(10,2),Petparticulars VARCHAR(200),Createdby BIGINT,Modifiedby BIGINT,Datecreated DATETIME2,Datemodified DATETIME2);

		MERGE INTO SystemStaffs AS target
        USING (SELECT 
                    JSON_VALUE(@JsonObjectData, '$.Userid') AS UserId,
                    JSON_VALUE(@JsonObjectData, '$.Firstname') AS FirstName,
                    JSON_VALUE(@JsonObjectData, '$.Lastname') AS LastName,
                    JSON_VALUE(@JsonObjectData, '$.Phonenumber') AS PhoneNumber,
                    JSON_VALUE(@JsonObjectData, '$.Emailaddress') AS UserName,
                    JSON_VALUE(@JsonObjectData, '$.Emailaddress') AS EmailAddress,
					JSON_VALUE(@JsonObjectData, '$.Roleid') AS RoleId,
					JSON_VALUE(@JsonObjectData, '$.Genderid') AS Genderid,
					JSON_VALUE(@JsonObjectData, '$.Maritalstatusid') AS Maritalstatusid,
                    JSON_VALUE(@JsonObjectData, '$.Passharsh') AS PassHarsh,
                    JSON_VALUE(@JsonObjectData, '$.Passwords') AS Passwords,
                    CAST(JSON_VALUE(@JsonObjectData, '$.Datecreated') AS DATETIME2) AS DateCreated,
                    JSON_VALUE(@JsonObjectData, '$.Createdby') AS CreatedBy,
                    JSON_VALUE(@JsonObjectData, '$.Modifiedby') AS ModifiedBy,
                    CAST(JSON_VALUE(@JsonObjectData, '$.Datemodified') AS DATETIME2) AS DateModified,
                    JSON_VALUE(@JsonObjectData, '$.Isactive') AS IsActive,
                    JSON_VALUE(@JsonObjectData, '$.Isdeleted') AS IsDeleted,
                    JSON_VALUE(@JsonObjectData, '$.Isdefault') AS IsDefault,
                    JSON_VALUE(@JsonObjectData, '$.Loginstatus') AS LoginStatus,
                    CAST(JSON_VALUE(@JsonObjectData, '$.Passwordresetdate') AS DATETIME2) AS PasswordResetDate,
                    JSON_VALUE(@JsonObjectData, '$.Parentid') AS ParentId,
                    JSON_VALUE(@JsonObjectData, '$.Userprofileimageurl') AS UserProfileImageUrl,
                    JSON_VALUE(@JsonObjectData, '$.Usercurriculumvitae') AS UserCurriculumVitae,
                    JSON_VALUE(@JsonObjectData, '$.Idnumber') AS IdNumber,
                    JSON_VALUE(@JsonObjectData, '$.Updateprofile') AS UpdateProfile,
                    JSON_VALUE(@JsonObjectData, '$.Extra') AS Extra,
                    JSON_VALUE(@JsonObjectData, '$.Extra1') AS Extra1,
                    JSON_VALUE(@JsonObjectData, '$.Extra2') AS Extra2,
                    JSON_VALUE(@JsonObjectData, '$.Extra3') AS Extra3,
                    JSON_VALUE(@JsonObjectData, '$.Extra4') AS Extra4,
                    JSON_VALUE(@JsonObjectData, '$.Extra5') AS Extra5,
                    CAST(JSON_VALUE(@JsonObjectData, '$.Lastlogin') AS DATETIME2) AS LastLogin
               ) AS source
        ON target.IdNumber = source.IdNumber
        WHEN MATCHED THEN
            UPDATE SET 
                FirstName = source.FirstName,
                LastName = source.LastName,
                PhoneNumber = source.PhoneNumber,
                UserName = source.EmailAddress,
                EmailAddress = source.EmailAddress,
				Genderid = source.Genderid,
				Maritalstatusid = source.Maritalstatusid,
                LoginStatus = source.LoginStatus,
                ParentId = source.ParentId,
                UserProfileImageUrl = source.UserProfileImageUrl,
                UserCurriculumVitae = source.UserCurriculumVitae,
                IdNumber = source.IdNumber,
                UpdateProfile = source.UpdateProfile,
                ModifiedBy = source.ModifiedBy,
                DateModified = source.DateModified
        WHEN NOT MATCHED THEN
            INSERT (FirstName, LastName, PhoneNumber, UserName, EmailAddress, RoleId, PassHarsh, Passwords, IsActive, IsDeleted, IsDefault, LoginStatus, PasswordResetDate, ParentId, UserProfileImageUrl, UserCurriculumVitae, IdNumber, UpdateProfile, Extra, Extra1, Extra2, Extra3, Extra4, Extra5, CreatedBy, ModifiedBy, DateModified, DateCreated, LastLogin)
            VALUES (source.FirstName, source.LastName, source.PhoneNumber, source.UserName, source.EmailAddress, source.RoleId, source.PassHarsh, source.Passwords, source.IsActive, source.IsDeleted, source.IsDefault, source.LoginStatus, source.PasswordResetDate, source.ParentId, source.UserProfileImageUrl, source.UserCurriculumVitae, source.IdNumber, source.UpdateProfile, source.Extra, source.Extra1, source.Extra2, source.Extra3, source.Extra4, source.Extra5, source.CreatedBy, source.ModifiedBy, source.DateModified, source.DateCreated, source.LastLogin)
            OUTPUT $action AS Action, inserted.UserId, inserted.FirstName + ' ' + inserted.LastName AS FullName, inserted.Passwords, inserted.PassHarsh, inserted.UserName, inserted.EmailAddress INTO @Systemstaffdata;
			
			 SET @UserId = (SELECT TOP 1 UserId FROM @Systemstaffdata);


       INSERT INTO @TempData
       SELECT Propertyhouseid, Isagency, @UserId AS Propertyhouseowner, Propertyhouseposter, Propertyhousename, Countyid, Subcountyid,Subcountywardid, Streetorlandmark, Contactdetails, Hashousedeposit, Rentdepositmonth, Hasagent, Propertyhousestatus,Watertypeid, Hashousewatermeter, Waterunitprice, Rentdueday, Vacantnoticeperiod, Monthlycollection,Rentutilityinclusive,Rentdepositreturndays,Allowpets,Rentingterms,Enddate,Numberofpets,Petdeposit,Petparticulars, Createdby,Modifiedby, Datecreated, Datemodified
       FROM OPENJSON(@JsonObjectdata)
       WITH (Propertyhouseid BIGINT '$.Propertyhouseid',Isagency BIT '$.Isagency',Propertyhouseposter BIGINT '$.Propertyhouseposter',Propertyhousename VARCHAR(255) '$.Propertyhousename',Countyid INT '$.Countyid',
       Subcountyid INT '$.Subcountyid',Subcountywardid INT '$.Subcountywardid',Streetorlandmark VARCHAR(255) '$.Streetorlandmark',Contactdetails VARCHAR(255) '$.Contactdetails',Hashousedeposit BIT '$.Hashousedeposit', Rentdepositmonth INT '$.Rentdepositmonth',Hasagent BIT '$.Hasagent',Propertyhousestatus VARCHAR(50) '$.Propertyhousestatus',Watertypeid BIGINT '$.Watertypeid',
	   Hashousewatermeter BIT '$.Hashousewatermeter',Waterunitprice DECIMAL(10,2) '$.Waterunitprice',Rentdueday INT '$.Rentdueday',Vacantnoticeperiod INT '$.Vacantnoticeperiod', Monthlycollection DECIMAL(18,2) '$.Monthlycollection',Rentutilityinclusive BIT '$.Rentutilityinclusive',Rentdepositreturndays INT '$.Rentdepositreturndays',Allowpets BIT '$.Allowpets',Rentingterms VARCHAR(20) '$.Rentingterms',Enddate DATETIME2 '$.Enddate',Numberofpets INT '$.Numberofpets',Petdeposit DECIMAL(10,2) '$.Petdeposit',Petparticulars VARCHAR(200) '$.Petparticulars',Createdby BIGINT '$.Createdby',Modifiedby BIGINT '$.Modifiedby',Datecreated DATETIME2 '$.Datecreated', Datemodified DATETIME2 '$.Datemodified');

       -- Perform the update
       UPDATE Systempropertyhouses SET Isagency = source.Isagency,Propertyhouseowner = source.Propertyhouseowner,Propertyhouseposter = source.Propertyhouseposter,Propertyhousename = source.Propertyhousename,Countyid = source.Countyid,Subcountyid = source.Subcountyid,Subcountywardid = source.Subcountywardid,Streetorlandmark = source.Streetorlandmark,Contactdetails = source.Contactdetails,
	   Hashousedeposit = source.Hashousedeposit,Rentdepositmonth = source.Rentdepositmonth,Hasagent = source.Hasagent,Propertyhousestatus = source.Propertyhousestatus,Watertypeid = source.Watertypeid,Hashousewatermeter = source.Hashousewatermeter, Waterunitprice = source.Waterunitprice,Rentdueday = source.Rentdueday,Vacantnoticeperiod = source.Vacantnoticeperiod,
	   Monthlycollection = source.Monthlycollection,Rentutilityinclusive= source.Rentutilityinclusive,Rentdepositreturndays= source.Rentdepositreturndays,Allowpets= source.Allowpets,Rentingterms= source.Rentingterms,Enddate= source.Enddate,Numberofpets= source.Numberofpets,Petdeposit= source.Petdeposit,Petparticulars= source.Petparticulars,Createdby = source.Createdby,Modifiedby = source.Modifiedby,Datecreated = source.Datecreated,Datemodified = source.Datemodified
       FROM Systempropertyhouses target
       JOIN @TempData source ON target.Propertyhouseid = source.Propertyhouseid;

       -- Perform the insert
       INSERT INTO Systempropertyhouses (Housecode, Isagency, Propertyhouseowner, Propertyhouseposter, Propertyhousename, Countyid, Subcountyid, Subcountywardid, Streetorlandmark, Contactdetails, Hashousedeposit, Rentdepositmonth, Hasagent, Propertyhousestatus, Watertypeid, Hashousewatermeter, Waterunitprice, Rentdueday, Vacantnoticeperiod, Monthlycollection,Rentutilityinclusive,Rentdepositreturndays,Allowpets,Rentingterms,Enddate,Numberofpets,Petdeposit,Petparticulars, Createdby, Modifiedby, Datecreated, Datemodified)
       SELECT 'MQP' + CONVERT(VARCHAR(70), NEXT VALUE FOR PropertyHouseCodeSequence), Isagency, Propertyhouseowner, Propertyhouseposter, Propertyhousename, Countyid, Subcountyid, Subcountywardid, Streetorlandmark, Contactdetails, Hashousedeposit, Rentdepositmonth, Hasagent, Propertyhousestatus, Watertypeid, Hashousewatermeter, Waterunitprice, Rentdueday, Vacantnoticeperiod, Monthlycollection,Rentutilityinclusive,Rentdepositreturndays,Allowpets,Rentingterms,Enddate,Numberofpets,Petdeposit,Petparticulars, Createdby, Modifiedby, Datecreated, Datemodified
       FROM @TempData source
       WHERE NOT EXISTS (SELECT 1 FROM Systempropertyhouses target WHERE target.Propertyhouseid = source.Propertyhouseid);
	   SET @Propertyhouseid = SCOPE_IDENTITY();

	   IF((SELECT Propertyhouseid FROM @TempData source)>=0)
	   BEGIN
	    UPDATE Systemstaffs SET Loginstatus=1 FROM Systemstaffs target JOIN @TempData source ON target.Userid = source.Propertyhouseowner;
	   END


		-- Assuming you have a table named Propertyhousesizes
		MERGE INTO Systempropertyhousesizes AS target
		USING (SELECT Systempropertyhousesizeid,ISNULL(@Propertyhouseid, Propertyhouseid) AS Propertyhouseid, Systemhousesizeid,Systempropertyhousesizeunits,Systempropertyhousesizewehave
		FROM OPENJSON(@JsonObjectdata, '$.Propertyhousesize')
		WITH (Systempropertyhousesizeid BIGINT '$.Systempropertyhousesizeid',Propertyhouseid BIGINT '$.Propertyhouseid',Systemhousesizeid INT '$.Systemhousesizeid',Systempropertyhousesizeunits INT '$.Systempropertyhousesizeunits',Systempropertyhousesizewehave BIT '$.Systempropertyhousesizewehave')) AS source
		ON target.Systempropertyhousesizeid = source.Systempropertyhousesizeid
		WHEN MATCHED THEN
		UPDATE SET target.Systemhousesizeid = source.Systemhousesizeid,target.Systempropertyhousesizeunits = source.Systempropertyhousesizeunits,target.Systempropertyhousesizewehave = source.Systempropertyhousesizewehave
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (Propertyhouseid,Systemhousesizeid,Systempropertyhousesizeunits,Systempropertyhousesizewehave)
		VALUES (source.Propertyhouseid,source.Systemhousesizeid,source.Systempropertyhousesizeunits,source.Systempropertyhousesizewehave);

	 -- Assuming you have a table named Propertyhousedepositfees
		MERGE INTO Systempropertyhousedepositfees AS target
		USING (SELECT Systempropertyhousedepositfeeid,ISNULL(@Propertyhouseid, Propertyhouseid) AS Propertyhouseid,Housedepositfeeid,Systempropertyhousedepositfeeamount,Systempropertyhousesizedepositfeewehave
		FROM OPENJSON(@JsonObjectdata, '$.Propertyhousedepositfee')
		WITH (Systempropertyhousedepositfeeid BIGINT '$.Systempropertyhousedepositfeeid',Propertyhouseid BIGINT '$.Propertyhouseid',Housedepositfeeid INT '$.Housedepositfeeid',Systempropertyhousedepositfeeamount DECIMAL(10, 2) '$.Systempropertyhousedepositfeeamount',Systempropertyhousesizedepositfeewehave BIT '$.Systempropertyhousesizedepositfeewehave')) AS source
		ON target.Systempropertyhousedepositfeeid = source.Systempropertyhousedepositfeeid
		WHEN MATCHED THEN
		UPDATE SET target.Housedepositfeeid = source.Housedepositfeeid,target.Systempropertyhousedepositfeeamount = source.Systempropertyhousedepositfeeamount,target.Systempropertyhousesizedepositfeewehave = source.Systempropertyhousesizedepositfeewehave
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (Propertyhouseid,Housedepositfeeid,Systempropertyhousedepositfeeamount,Systempropertyhousesizedepositfeewehave)
		VALUES (source.Propertyhouseid,source.Housedepositfeeid,source.Systempropertyhousedepositfeeamount,source.Systempropertyhousesizedepositfeewehave);

		-- Assuming you have a table named Propertyhousebanks
		MERGE INTO Systempropertybankaccounts AS target
		USING (SELECT Systempropertybankaccountid,ISNULL(@Propertyhouseid, Propertyhouseid) AS Propertyhouseid,Systembankid,Systempropertybankaccount,Systempropertyhousebankwehave
		FROM OPENJSON(@JsonObjectdata, '$.Propertyhousebankingdetail')
		WITH (Systempropertybankaccountid BIGINT '$.Systempropertybankaccountid',Propertyhouseid BIGINT '$.Propertyhouseid',Systembankid INT '$.Systembankid',Systempropertybankaccount VARCHAR(20) '$.Systempropertybankaccount',Systempropertyhousebankwehave BIT '$.Systempropertyhousebankwehave')) AS source
		ON target.Systempropertybankaccountid = source.Systempropertybankaccountid
		WHEN MATCHED THEN
		UPDATE SET target.Systembankid = source.Systembankid,target.Systempropertybankaccount = source.Systempropertybankaccount,target.Systempropertyhousebankwehave = source.Systempropertyhousebankwehave
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (Propertyhouseid,Systembankid,Systempropertybankaccount,Systempropertyhousebankwehave)
		VALUES (source.Propertyhouseid,source.Systembankid,source.Systempropertybankaccount,source.Systempropertyhousebankwehave);

		-- Assuming you have a table named Propertyhousebenefits
		MERGE INTO Systempropertyhousebenefits AS target
		USING (SELECT Systempropertyhousebenefitid,ISNULL(@Propertyhouseid, Propertyhouseid) AS Propertyhouseid,Housebenefitid,Systempropertyhousebenefitwehave
		FROM OPENJSON(@JsonObjectdata, '$.Propertyhousebenefit')
		WITH (Systempropertyhousebenefitid BIGINT '$.Systempropertyhousebenefitid',Propertyhouseid INT '$.Propertyhouseid',Housebenefitid INT '$.Housebenefitid',Systempropertyhousebenefitwehave BIT '$.Systempropertyhousebenefitwehave')) AS source
		ON target.Systempropertyhousebenefitid = source.Systempropertyhousebenefitid
		WHEN MATCHED THEN
		UPDATE SET target.Housebenefitid = source.Housebenefitid,target.Systempropertyhousebenefitwehave = source.Systempropertyhousebenefitwehave
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (Propertyhouseid,Housebenefitid,Systempropertyhousebenefitwehave)
		VALUES (source.Propertyhouseid,source.Housebenefitid,source.Systempropertyhousebenefitwehave);

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