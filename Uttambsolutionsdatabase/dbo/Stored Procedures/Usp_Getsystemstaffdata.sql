CREATE PROCEDURE [dbo].[Usp_Getsystemstaffdata]
@Page INT,
@PageSize INT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Success';
			BEGIN
	
		BEGIN TRY
		--validate	
		
		BEGIN TRANSACTION;
		SELECT Systemstaff.Userid,Systemstaff.Firstname,Systemstaff.Lastname,Systemstaff.Phonenumber,Systemstaff.Username,Systemstaff.Emailaddress,Systemstaff.Genderid,Systemstaff.Maritalstatusid,Systemstaff.Roleid,Systemrole.Rolename,Systemstaff.Passharsh,Systemstaff.Passwords,Systemstaff.Isactive,Systemstaff.Isdeleted,Systemstaff.Isdefault,Systemstaff.Loginstatus,Systemstaff.Passwordresetdate,Systemstaff.Parentid,Systemstaff.Userprofileimageurl,Systemstaff.Usercurriculumvitae,Systemstaff.Idnumber,Systemstaff.Updateprofile,Systemstaff.Extra,Systemstaff.Extra1,Systemstaff.Extra2,Systemstaff.Extra3,Systemstaff.Extra4,Systemstaff.Extra5,Systemstaff.Createdby,Systemstaff.Modifiedby,Systemstaff.Lastlogin,Systemstaff.Datemodified,Systemstaff.Datecreated 
		FROM Systemstaffs Systemstaff 
		INNER JOIN Systemroles Systemrole ON Systemstaff.RoleId=Systemrole.RoleId
		ORDER BY Systemstaff.Datecreated
		OFFSET @Page ROWS
		FETCH NEXT @PageSize ROWS ONLY;

	    Set @RespMsg ='Success'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage

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
