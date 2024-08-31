CREATE PROCEDURE [dbo].[Usp_verifysystemuser]
	@Username varchar(100),
    @StaffDetails NVARCHAR(MAX) OUTPUT
AS
BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'login success';
			BEGIN
				--validate	
				IF NOT EXISTS(SELECT Userid FROM SystemStaffs WHERE Username=@Username)
				Begin
					--Select  1 as RespStatus, 'Invalid Emailaddress or User does not Exist!' as RespMessage,@StaffDetails AS StaffDetails
					SET @StaffDetails = (SELECT 1 as RespStatus, 'Invalid Emailaddress or User does not Exist!' as RespMessage,@StaffDetails AS StaffDetails FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER);
					Return
				End

				IF EXISTS(SELECT Userid FROM SystemStaffs WHERE Username=@Username AND IsActive=0)
				Begin
					SET @StaffDetails = (Select  1 as RespStatus, 'Inactive Account Contact Admin!' as RespMessage,@StaffDetails AS StaffDetails FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER);
					Return
				End
				IF EXISTS(SELECT Userid FROM SystemStaffs WHERE Username=@Username AND IsDeleted=1)
				Begin
					SET @StaffDetails = (Select  1 as RespStatus, 'Account Deleted Contact Admin!' as RespMessage,@StaffDetails AS StaffDetails FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER);
					Return
				End
				IF EXISTS(SELECT Userid FROM SystemStaffs WHERE Username=@Username AND LoginStatus=4)
				Begin
					SET @StaffDetails = (Select  1 as RespStatus, 'Account Blocked Contact Admin!' as RespMessage,@StaffDetails AS StaffDetails FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER);
					Return
				End
		
			BEGIN 
					SET @StaffDetails = (
						SELECT
						@RespStat AS RespStatus,
						@RespMsg AS RespMessage,
						(SELECT Systemstaff.Userid,Systemstaff.Firstname,Systemstaff.Lastname,Systemstaff.Firstname +' '+ Systemstaff.Lastname AS Fullname,Systemstaff.Phonenumber,Systemstaff.Username,
							Systemstaff.Emailaddress,Systemstaff.Genderid ,Systemstaff.Maritalstatusid,Systemstaff.Roleid,Systemstaff.Passharsh,Systemstaff.Passwords,Systemstaff.Isactive,Systemstaff.Isdeleted,Systemstaff.Isdefault,
							Systemstaff.Loginstatus,ISNULL(Systemstaffdesignation.Staffdesignation,'System Admin') AS Designation,Systemstaff.Passwordresetdate,Systemstaff.Parentid,Systemstaff.Userprofileimageurl,Systemstaff.Usercurriculumvitae,Systemstaff.Idnumber,Systemstaff.Updateprofile,ISNULL(Systemstaffsaccount.Accountnumber,0) AS Accountnumber,0 AS Walletbalance,
							Systemrole.Rolename,Systemrole.RoleDescription,Systemrole.Tenantid,Systemstaff.Createdby,Systemstaff.Modifiedby,Systemstaff.Lastlogin,Systemstaff.Datemodified,Systemstaff.Datecreated
							FROM Systemstaffs Systemstaff
							INNER JOIN Systemroles Systemrole ON Systemstaff.Roleid=Systemrole.Roleid
							LEFT JOIN Systemstaffsaccount Systemstaffsaccount ON Systemstaff.Userid=Systemstaffsaccount.Userid
							LEFT JOIN Systemstaffdesignations Systemstaffdesignation ON Systemstaff.Userid=Systemstaffdesignation.Systemstaffid
						WHERE Systemstaff.Username = @Username
						FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
						) AS Usermodel
						FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
					);
			END
		END
END