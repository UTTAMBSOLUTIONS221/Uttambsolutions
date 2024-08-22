CREATE PROCEDURE [dbo].[Usp_Registersystemstaffdata]
@JsonObjectData VARCHAR(MAX)
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '',
            @UserId BIGINT,
            @RoleId INT,
            @AccountId INT,
            @AccountNumber INT;

    BEGIN TRY
        -- Validate RoleId
        SET @RoleId = JSON_VALUE(@JsonObjectData, '$.Roleid');
        IF @RoleId IS NULL OR NOT EXISTS(SELECT Roleid FROM Systemroles WHERE Roleid = @RoleId)
        BEGIN
            -- Check if "Default User" role exists
            IF NOT EXISTS(SELECT RoleId FROM Systemroles WHERE RoleName = 'Default User')
            BEGIN
                INSERT INTO Systemroles (Rolename, RoleDescription, Tenantid, Isdefault, Isactive, Isdeleted, Createdby, Modifiedby, Datemodified, Datecreated)
                VALUES ('Default User', 'Default User', 1, 1, 1, 0, 1, 1, GETDATE(), GETDATE());
            END

            -- Get the RoleId of "Default User"
            SELECT @RoleId = RoleId FROM Systemroles WHERE Rolename = 'Default User';
        END

        -- Check for duplicate email, username, or phone number
		IF(JSON_VALUE(@JsonObjectData, '$.Userid')=0)
		BEGIN
        IF EXISTS(SELECT UserId FROM SystemStaffs WHERE EmailAddress = JSON_VALUE(@JsonObjectData, '$.Emailaddress'))
        BEGIN
            SELECT 1 AS RespStatus, 'Similar Email Address exists! Contact Admin!' AS RespMessage;
            RETURN;
        END
        IF EXISTS(SELECT UserId FROM SystemStaffs WHERE UserName = JSON_VALUE(@JsonObjectData, '$.Username'))
        BEGIN
            SELECT 1 AS RespStatus, 'Similar username exists! Contact Admin!' AS RespMessage;
            RETURN;
        END
        IF EXISTS(SELECT UserId FROM SystemStaffs WHERE PhoneNumber = JSON_VALUE(@JsonObjectData, '$.Phonenumber'))
        BEGIN
            SELECT 1 AS RespStatus, 'Similar PhoneNumber exists! Contact Admin!' AS RespMessage;
            RETURN;
        END
		END
        BEGIN TRANSACTION;
        DECLARE @Systemstaffdata TABLE (Action VARCHAR(100), UserId BIGINT, FullName VARCHAR(140), Passwords VARCHAR(100), PassHarsh VARCHAR(100), UserName VARCHAR(100), EmailAddress VARCHAR(100));

        -- Use MERGE statement for insert/update
        MERGE INTO SystemStaffs AS target
        USING (SELECT 
                    JSON_VALUE(@JsonObjectData, '$.Userid') AS UserId,
                    JSON_VALUE(@JsonObjectData, '$.Firstname') AS FirstName,
                    JSON_VALUE(@JsonObjectData, '$.Lastname') AS LastName,
                    JSON_VALUE(@JsonObjectData, '$.Phonenumber') AS PhoneNumber,
                    JSON_VALUE(@JsonObjectData, '$.Username') AS UserName,
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
        ON target.UserId = source.UserId
        WHEN MATCHED THEN
            UPDATE SET 
                FirstName = source.FirstName,
                LastName = source.LastName,
                PhoneNumber = source.PhoneNumber,
                UserName = source.EmailAddress,
                EmailAddress = source.EmailAddress,
                RoleId = source.RoleId,
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
        -- Insert into Systemstaffsaccount if it was an insert action
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
		ELSE
		BEGIN
		  IF NOT EXISTS(SELECT Userid FROM Systemstaffkins WHERE Userid =JSON_VALUE(@JsonObjectData, '$.Userid'))
		  BEGIN
		   INSERT INTO Systemstaffkins(Userid,Kinname,Kinphonenumber,Kinrelationshipid,Datecreated)
		   SELECT JSON_VALUE(@JsonObjectData, '$.Userid'),JSON_VALUE(@JsonObjectData, '$.Kinname'),JSON_VALUE(@JsonObjectData, '$.Kinphonenumber'),JSON_VALUE(@JsonObjectData, '$.Kinrelationshipid'),CAST(JSON_VALUE(@JsonObjectData, '$.Datemodified') AS DATETIME2);
		  END
		END
		IF EXISTS(SELECT 1 FROM @Systemstaffdata WHERE Action = 'INSERT')
        BEGIN
			IF NOT EXISTS(SELECT Systemstaffid FROM Systemstaffdesignations WHERE Systemstaffid=@UserId AND JSON_VALUE(@JsonObjectData, '$.Designation') IS NOT NULL)
			BEGIN
				INSERT INTO Systemstaffdesignations(Systemstaffid,Staffdesignation,Datecreated)
				SELECT UserId,JSON_VALUE(@JsonObjectData, '$.Designation'),CAST(JSON_VALUE(@JsonObjectData, '$.Datecreated') AS DATETIME2) FROM @Systemstaffdata
			END
		END
		IF EXISTS(SELECT 1 FROM @Systemstaffdata WHERE Action = 'INSERT')
        BEGIN
			IF NOT EXISTS(SELECT Systemstaffid FROM Systemstaffdesignations WHERE Systemstaffid=@UserId)
			BEGIN
				INSERT INTO Systemstafftermandservices(Systemstaffid,Accepttermandservices,Datecreated)
				SELECT UserId,JSON_VALUE(@JsonObjectData, '$.Accepttermsandcondition'),CAST(JSON_VALUE(@JsonObjectData, '$.Datecreated') AS DATETIME2) FROM @Systemstaffdata
			END
		END

        SET @RespMsg = 'Success';
        SET @RespStat = 0; 
        COMMIT TRANSACTION;

        SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage, Action AS Data1,UserId AS Data2, FullName AS Data3,Passwords AS Data4,PassHarsh AS Data5, UserName AS Data6, EmailAddress AS Data7 FROM @Systemstaffdata;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT '';
        PRINT 'Error ' + ERROR_MESSAGE();
        SELECT 2 AS RespStatus, '0 - Error(s) Occurred: ' + ERROR_MESSAGE() AS RespMessage;
    END CATCH

    SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;
    RETURN;
END
