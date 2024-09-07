CREATE PROCEDURE [dbo].[Usp_Verifyforgotpasswordsystemstaff]
	@JsonObjectData varchar(200),
    @StaffDetails NVARCHAR(MAX) OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 	       
	        @Passwordstatus VARCHAR(40) = 'Passwordgotten',
			@RespStat int = 0,
			@RespMsg varchar(150) = 'login success';
		  
	BEGIN
		BEGIN TRY	
		--validate
				IF NOT EXISTS(SELECT Androidid FROM Systemuserdevices WHERE Androidid=JSON_VALUE(@JsonObjectData, '$.Androidid'))
				BEGIN
					SET @StaffDetails = (SELECT 1 as RespStatus, 'Mobile phone do not exist. Contact Admin!' as RespMessage,@StaffDetails AS StaffDetails FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER);
					Return
				END
				IF NOT EXISTS(SELECT Userid FROM SystemStaffs WHERE Username=JSON_VALUE(@JsonObjectData, '$.Emailaddress'))
				Begin
					--Select  1 as RespStatus, 'Invalid Emailaddress or User does not Exist!' as RespMessage,@StaffDetails AS StaffDetails
					SET @StaffDetails = (SELECT 1 as RespStatus, 'Invalid Emailaddress or User does not Exist!' as RespMessage,@StaffDetails AS StaffDetails FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER);
					Return
				End

				IF EXISTS(SELECT Userid FROM SystemStaffs WHERE Username=JSON_VALUE(@JsonObjectData, '$.Emailaddress') AND IsActive=0)
				Begin
					SET @StaffDetails = (Select  1 as RespStatus, 'Inactive Account Contact Admin!' as RespMessage,@StaffDetails AS StaffDetails FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER);
					Return
				End
				IF EXISTS(SELECT Userid FROM SystemStaffs WHERE Username=JSON_VALUE(@JsonObjectData, '$.Emailaddress') AND IsDeleted=1)
				Begin
					SET @StaffDetails = (Select  1 as RespStatus, 'Account Deleted Contact Admin!' as RespMessage,@StaffDetails AS StaffDetails FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER);
					Return
				End
				IF EXISTS(SELECT Userid FROM SystemStaffs WHERE Username=JSON_VALUE(@JsonObjectData, '$.Emailaddress') AND LoginStatus=4)
				Begin
					SET @StaffDetails = (Select  1 as RespStatus, 'Account Blocked Contact Admin!' as RespMessage,@StaffDetails AS StaffDetails FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER);
					Return
				End
		BEGIN TRANSACTION;
		
			IF((SELECT JSON_VALUE(@JsonObjectData, '$.Userid'))>0)
			BEGIN

			 MERGE INTO SystemStaffs AS target
             USING (SELECT JSON_VALUE(@JsonObjectData, '$.Userid') AS UserId,JSON_VALUE(@JsonObjectData, '$.Passharsh') AS PassHarsh,JSON_VALUE(@JsonObjectData, '$.Passwords') AS Passwords,GETDATE() AS Passwordresetdate) AS source
			 ON target.UserId = source.UserId WHEN MATCHED THEN
			 UPDATE SET Passwords = source.Passwords,Passharsh = source.Passharsh,Passwordresetdate = source.Passwordresetdate;


				--UPDATE SystemStaffs SET Passharsh = JSON_VALUE(@JsonObjectData, '$.Passharsh'),Passwords = JSON_VALUE(@JsonObjectData, '$.Passwords'),Passwordresetdate = GETDATE() WHERE Userid = JSON_VALUE(@JsonObjectData, '$.Userid');
				SET @Passwordstatus = 'Passwordupdated';
			END
			SET @StaffDetails = (
				SELECT
				(SELECT Systemstaff.Userid,Systemstaff.Firstname,Systemstaff.Lastname,Systemstaff.Firstname +' '+ Systemstaff.Lastname AS Fullname,Systemstaff.Phonenumber,Systemstaff.Username,
					Systemstaff.Emailaddress,JSON_VALUE(@JsonObjectData, '$.Androidid') AS Androidid,@Passwordstatus AS Passwordstatus,Systemstaff.Genderid ,Systemstaff.Maritalstatusid,Systemstaff.Roleid,Systemstaff.Isactive,Systemstaff.Isdeleted,Systemstaff.Isdefault,
					Systemstaff.Loginstatus,ISNULL(Systemstaffdesignation.Staffdesignation,'System Admin') AS Designation,Systemstaff.Passwordresetdate,Systemstaff.Parentid,Systemstaff.Userprofileimageurl,Systemstaff.Usercurriculumvitae,Systemstaff.Idnumber,Systemstaff.Updateprofile,ISNULL(Systemstaffsaccount.Accountnumber,0) AS Accountnumber,0 AS Walletbalance,
					Systemrole.Rolename,Systemrole.RoleDescription,Systemrole.Tenantid,Systemstaff.Createdby,Systemstaff.Modifiedby,Systemstaff.Lastlogin,Systemstaff.Datemodified,Systemstaff.Datecreated
					FROM Systemstaffs Systemstaff
					INNER JOIN Systemroles Systemrole ON Systemstaff.Roleid=Systemrole.Roleid
					LEFT JOIN Systemstaffsaccount Systemstaffsaccount ON Systemstaff.Userid=Systemstaffsaccount.Userid
					LEFT JOIN Systemstaffdesignations Systemstaffdesignation ON Systemstaff.Userid=Systemstaffdesignation.Systemstaffid
				WHERE Systemstaff.Username = JSON_VALUE(@JsonObjectData, '$.Emailaddress')
				FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
				) AS Data
				FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			);	Set @RespMsg ='Success'
		Set @RespStat =0; 
		COMMIT TRANSACTION;
		Select @RespStat as RespStatus, @RespMsg as RespMessage,@StaffDetails AS StaffDetails;

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