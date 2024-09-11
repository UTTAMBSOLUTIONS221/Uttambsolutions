CREATE PROCEDURE [dbo].[Usp_Registerpropertyhouseroomdata]
@JsonObjectdata VARCHAR(MAX)
AS
BEGIN
   BEGIN
	DECLARE @RespStat int = 0,
			@RespMsg varchar(150) = '',
			@UserId BIGINT,
			@RoleId INT,
            @AccountId INT,
            @AccountNumber INT,
			@Systempropertyhouseroomid  bigint,
			@Systempropertyhousetenantentryid bigint,
			@Isnewtenant BIT =1;
		  
	BEGIN
		BEGIN TRY	
		--Validate
		IF EXISTS(SELECT Systempropertyhousetenantid FROM Systempropertyhouseroomstenant WHERE Systempropertyhousetenantid = JSON_VALUE(@JsonObjectdata, '$.Tenantid') AND Systempropertyhouseroomid != JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroomid') AND Occupationalstatus=2)
		BEGIN
			SELECT 1 AS RespStatus,'Tenant has not given vacating notice to current residence' AS RespMessage;
			return;
		END

		BEGIN TRANSACTION;
		    DECLARE @Systempropertyhouseroomsdata TABLE (Systempropertyhouseroomid BIGINT);
			DECLARE @Systempropertyhouseroomstenantdata TABLE (Systempropertyhousetenantentryid BIGINT);
			DECLARE @InsertedIDs TABLE(Propertychecklistid BIGINT,Fixtureid INT);
			DECLARE @Systemstaffdata TABLE (Action VARCHAR(100), UserId BIGINT, FullName VARCHAR(140), Passwords VARCHAR(100), PassHarsh VARCHAR(100), UserName VARCHAR(100), EmailAddress VARCHAR(100));
			SET @RoleId = (SELECT RoleId FROM Systemroles WHERE Rolename = 'Maqaoplus Property House Tenant');
       
		MERGE INTO SystemStaffs AS target
        USING (SELECT 
                    JSON_VALUE(@JsonObjectData, '$.Userid') AS UserId,
                    JSON_VALUE(@JsonObjectData, '$.Firstname') AS FirstName,
                    JSON_VALUE(@JsonObjectData, '$.Lastname') AS LastName,
                    JSON_VALUE(@JsonObjectData, '$.Phonenumber') AS PhoneNumber,
                    JSON_VALUE(@JsonObjectData, '$.Emailaddress') AS UserName,
                    JSON_VALUE(@JsonObjectData, '$.Emailaddress') AS EmailAddress,
					@RoleId AS RoleId,
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
			 IF EXISTS(SELECT 1 FROM @Systemstaffdata WHERE Action = 'INSERT')
			BEGIN
				INSERT INTO Systemstaffsaccount (Userid, AccountNumber, Datecreated, Datemodified)
				SELECT UserId, NEXT VALUE FOR AccountNumberSequence, CAST(JSON_VALUE(@JsonObjectData, '$.Datecreated') AS DATETIME2), CAST(JSON_VALUE(@JsonObjectData, '$.Datemodified') AS DATETIME2)
				FROM @Systemstaffdata
				WHERE Action = 'INSERT';

				SET @AccountId = SCOPE_IDENTITY();
				SET @AccountNumber = (SELECT AccountNumber FROM Systemstaffsaccount WHERE AccountId = @AccountId);
				INSERT INTO ChartofAccounts 
				VALUES (@AccountNumber, 12);
			END


			MERGE INTO Systempropertyhouserooms AS target
			USING (
			SELECT Systempropertyhouseroomid,Systempropertyhouseid,Systempropertyhousesizeid,Systempropertyhousesizename,Systempropertyhousesizerent,Isvacant,Isunderrenovation,Isshop,Isgroundfloor,Hasbalcony,Forcaretaker,Kitchentypeid
			FROM OPENJSON(@JsonObjectdata)
			WITH (Systempropertyhouseroomid BIGINT '$.Systempropertyhouseroomid',Systempropertyhouseid BIGINT '$.Systempropertyhouseid',Systempropertyhousesizeid BIGINT '$.Systempropertyhousesizeid',
			Systempropertyhousesizename VARCHAR(100) '$.Systempropertyhousesizename',Systempropertyhousesizerent DECIMAL(10,2) '$.Systempropertyhousesizerent',Isvacant BIT '$.Isvacant',Isunderrenovation BIT '$.Isunderrenovation',Isshop BIT '$.Isshop',Isgroundfloor BIT '$.Isgroundfloor',Hasbalcony BIT '$.Hasbalcony',Forcaretaker BIT '$.Forcaretaker',Kitchentypeid INT '$.Kitchentypeid'
			)) AS source
			ON target.Systempropertyhouseroomid = source.Systempropertyhouseroomid
			WHEN MATCHED THEN
			UPDATE SET target.Systempropertyhousesizeid =(select Systempropertyhousesize.Systempropertyhousesizeid from Systempropertyhousesizes Systempropertyhousesize WHERE Systempropertyhousesize.Systemhousesizeid=source.Systempropertyhousesizeid AND Systempropertyhousesize.Propertyhouseid=source.Systempropertyhouseid),target.Systempropertyhousesizename = source.Systempropertyhousesizename,target.Systempropertyhousesizerent = source.Systempropertyhousesizerent,
			target.Isunderrenovation = source.Isunderrenovation,target.Isshop = source.Isshop ,target.Isgroundfloor = source.Isgroundfloor,target.Hasbalcony = source.Hasbalcony,target.Forcaretaker = source.Forcaretaker,target.Kitchentypeid = source.Kitchentypeid
			WHEN NOT MATCHED BY TARGET THEN
			INSERT (Systempropertyhouseid,Systempropertyhousesizeid,Systempropertyhousesizename,Systempropertyhousesizerent,Systempropertyhousesizedeposit,Isvacant,Isunderrenovation ,Isshop,Isgroundfloor,Hasbalcony,Forcaretaker,Kitchentypeid)
			VALUES (source.Systempropertyhouseid,(select Systempropertyhousesize.Systempropertyhousesizeid from Systempropertyhousesizes Systempropertyhousesize WHERE Systempropertyhousesize.Systemhousesizeid=source.Systempropertyhousesizeid AND Systempropertyhousesize.Propertyhouseid=source.Systempropertyhouseid),source.Systempropertyhousesizename,source.Systempropertyhousesizerent,0,0,source.Isunderrenovation,source.Isshop,source.Isgroundfloor,source.Hasbalcony,source.Forcaretaker,source.Kitchentypeid)
		    OUTPUT inserted.Systempropertyhouseroomid INTO @Systempropertyhouseroomsdata;
			SET @Systempropertyhouseroomid = (SELECT TOP 1 Systempropertyhouseroomid FROM @Systempropertyhouseroomsdata);

			--set room vacant if exists
			IF EXISTS (SELECT Systempropertyhouseroomid FROM Systempropertyhouseroomstenant WHERE Systempropertyhousetenantid = JSON_VALUE(@JsonObjectdata, '$.Tenantid') AND Isoccupant=1 AND Occupationalstatus=1)
            BEGIN 
			   UPDATE Systempropertyhouseroomstenant SET Isoccupant=0,Occupationalstatus=0,Vacateddate =GETDATE() WHERE Systempropertyhouseroomid = (SELECT Systempropertyhouseroomid FROM Systempropertyhouseroomstenant WHERE Systempropertyhousetenantid = JSON_VALUE(@JsonObjectdata, '$.Tenantid'));
			END
			IF NOT EXISTS( SELECT Systempropertyhousetenantentryid FROM Systempropertyhouseroomstenant WHERE Systempropertyhousetenantid= JSON_VALUE(@JsonObjectdata, '$.Tenantid'))
			BEGIN
			 UPDATE Systemstaffs SET Loginstatus=1 WHERE Userid=JSON_VALUE(@JsonObjectdata, '$.Tenantid');
			END

			--insert tenant to tenants table
			MERGE INTO Systempropertyhouseroomstenant AS target
			USING(
			SELECT @UserId AS Systempropertyhousetenantid,Systempropertyhouseroomid,Roomoccupant,Roomoccupantdetail,Createdby,Modifiedby,Datecreated,Datemodified
			FROM OPENJSON(@JsonObjectdata)
			WITH (Systempropertyhouseroomid BIGINT '$.Systempropertyhouseroomid',Roomoccupant INT '$.Roomoccupant',Roomoccupantdetail VARCHAR(200) '$.Roomoccupantdetail',Createdby BIGINT '$.Createdby',Modifiedby BIGINT '$.Createdby',  Datecreated DATETIME2 '$.Datecreated',Datemodified DATETIME2 '$.Datecreated')) AS source
			ON target.Systempropertyhousetenantid = source.Systempropertyhousetenantid AND target.Systempropertyhouseroomid = source.Systempropertyhouseroomid AND target.Isoccupant = 1 AND Occupationalstatus= 2
			WHEN MATCHED THEN
			UPDATE SET target.Roomoccupant =source.Roomoccupant,target.Roomoccupantdetail = source.Roomoccupantdetail	
			WHEN NOT MATCHED THEN
			INSERT (Systempropertyhousetenantid, Systempropertyhouseroomid,Isoccupant,Occupationalstatus,Isnewtenant,Roomoccupant,Roomoccupantdetail, Createdby, Modifiedby,Vacateddate, Datecreated, Datemodified)
			VALUES (source.Systempropertyhousetenantid, source.Systempropertyhouseroomid,1,2,@Isnewtenant,source.Roomoccupant, source.Roomoccupantdetail, source.Createdby, source.Modifiedby,source.Datecreated, source.Datecreated, source.Datemodified)
			OUTPUT inserted.Systempropertyhousetenantentryid INTO @Systempropertyhouseroomstenantdata;
		    SET @Systempropertyhousetenantentryid = (SELECT TOP 1 Systempropertyhousetenantentryid FROM @Systempropertyhouseroomstenantdata);

			MERGE INTO Systempropertyhousechecklists AS target
			USING (SELECT Propertychecklistid,Propertyhouseroomid,Fixtureid,Fixtureunits,Fixturestatusid,Createdby,Datecreated
			FROM OPENJSON(@JsonObjectdata, '$.Roomfixtures') WITH (Propertychecklistid BIGINT '$.Propertychecklistid',Propertyhouseroomid BIGINT '$.Propertyhouseroomid',
			Fixtureid INT '$.Fixtureid',Fixtureunits INT '$.Fixtureunits',Fixturestatusid INT '$.Fixturestatusid',Createdby BIGINT '$.Createdby',Datecreated DATETIME2 '$.Datecreated')) AS source
			ON target.Propertychecklistid = source.Propertychecklistid AND target.Propertyhouseroomid = source.Propertyhouseroomid AND target.Fixtureid = source.Fixtureid
			WHEN MATCHED THEN
			UPDATE SET target.Fixtureunits = source.Fixtureunits,target.Fixturestatusid = source.Fixturestatusid,target.Createdby = source.Createdby,target.Datecreated = source.Datecreated
			WHEN NOT MATCHED BY TARGET THEN
			INSERT (Propertyhouseroomid,Fixtureid,Fixtureunits,Fixturestatusid,Createdby,Datecreated)
			VALUES (source.Propertyhouseroomid,source.Fixtureid,source.Fixtureunits,source.Fixturestatusid,source.Createdby,source.Datecreated)
			OUTPUT inserted.Propertychecklistid,inserted.Fixtureid INTO @InsertedIDs;


			INSERT INTO Systemfixturestatushist (Propertychecklistid, Fixturestatusid, Fixtureunits)
			SELECT i.Propertychecklistid, s.Fixturestatusid, s.Fixtureunits FROM @InsertedIDs i 
			JOIN OPENJSON(@JsonObjectdata, '$.Roomfixtures') WITH (Propertychecklistid BIGINT '$.Propertychecklistid',Fixturestatusid INT '$.Fixturestatusid', Fixtureunits INT '$.Fixtureunits') AS s
			ON i.Propertychecklistid = s.Propertychecklistid WHERE s.Fixtureunits > 0 AND (NOT EXISTS (
			SELECT 1 FROM Systemfixturestatushist hist WHERE hist.Propertychecklistid = i.Propertychecklistid AND hist.Fixturestatusid = s.Fixturestatusid AND hist.Fixtureunits = s.Fixtureunits) 
			OR s.Fixturestatusid <> (SELECT TOP 1 hist.Fixturestatusid FROM Systemfixturestatushist hist WHERE hist.Propertychecklistid = i.Propertychecklistid ORDER BY hist.Datecreated DESC)
			OR s.Fixtureunits <> (SELECT TOP 1 hist.Fixtureunits  FROM Systemfixturestatushist hist  WHERE hist.Propertychecklistid = i.Propertychecklistid ORDER BY hist.Datecreated DESC));

			--update any other entry for the tenant to have vacated
			UPDATE Systempropertyhouseroomstenant SET Isoccupant=0,Occupationalstatus=0  WHERE Systempropertyhousetenantid = JSON_VALUE(@JsonObjectdata, '$.Tenantid');
			--update the current entry for the tenant to have resided
			UPDATE Systempropertyhouseroomstenant SET Isoccupant=1,Occupationalstatus=2 WHERE Systempropertyhousetenantentryid = @Systempropertyhousetenantentryid AND Systempropertyhousetenantid = JSON_VALUE(@JsonObjectdata, '$.Tenantid');
			--update house room to not vacant anymore 
		    UPDATE Systempropertyhouserooms SET Isvacant=0 WHERE Systempropertyhouseroomid = JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroomid');
           

		   IF ((SELECT Systempropertyhouse.Hashousewatermeter FROM Systempropertyhouses Systempropertyhouse INNER JOIN Systempropertyhouserooms Systempropertyhouseroom ON Systempropertyhouse.Propertyhouseid=Systempropertyhouseroom.Systempropertyhouseid WHERE Systempropertyhouseroom.Systempropertyhouseroomid=JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroomid'))=1)
		   BEGIN
			   IF NOT EXISTS (SELECT 1 FROM Systempropertyhouseroommeters WHERE Systempropertyhouseroomid = JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroomid') AND YEAR(Datecreated) = YEAR(TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Datecreated') AS DATETIME2)) AND MONTH(Datecreated) = MONTH(TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Datecreated') AS DATETIME2)))
				BEGIN
				   -- Insert into Systempropertyhouseroommeters table with conditions
				   INSERT INTO Systempropertyhouseroommeters (Systempropertyhouseroomid, Systempropertyhouseroommeternumber, Openingmeter, Movedmeter, Closingmeter, Consumedamount, Createdby, Datecreated)
				   SELECT JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroomid') AS Systempropertyhouseroomid,JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroommeternumber') AS Systempropertyhouseroommeternumber,TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Openingmeter') AS FLOAT) AS Openingmeter,TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Movedmeter') AS FLOAT) AS Movedmeter,TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Closingmeter') AS FLOAT) AS Closingmeter,TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Consumedamount') AS FLOAT) AS Consumedamount,JSON_VALUE(@JsonObjectdata, '$.Createdby') AS Createdby,TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Datecreated') AS DATETIME2) AS Datecreated
				   WHERE TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Closingmeter') AS FLOAT) > 0 OR TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Movedmeter') AS FLOAT) > 0 OR TRY_CAST(JSON_VALUE(@JsonObjectdata, '$.Consumedamount') AS FLOAT) > 0;
				END
		   END

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