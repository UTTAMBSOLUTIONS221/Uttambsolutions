CREATE PROCEDURE [dbo].[Usp_Getsystemuserprofiledata]
    @Userid BIGINT,
	@UserProfileData VARCHAR(MAX)  OUTPUT
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
		  SET @UserProfileData = (
			   SELECT a.Userid,a.Firstname,a.Lastname,a.Firstname+' '+a.Lastname AS Fullname,a.Phonenumber,a.Username AS Username,a.Emailaddress,a.Roleid,a.Isactive,a.Isdeleted,a.Loginstatus,a.Userprofileimageurl,a.Usercurriculumvitae,a.Passwordresetdate,a.Extra,a.Extra1,a.Extra2,a.Extra3,a.Extra4,a.Extra5,a.Createdby,a.Modifiedby,a.Lastlogin,a.Datemodified,a.Datecreated,
			    (SELECT org.Organizationid,org.Organizationowner,org.Organizationname,org.OrganizationPhone,org.OrganizationEmail,org.Organizationdescription,org.Organizationlogo,org.Organizationtypeid,org.Organizationstatus,org.Datecreated FROM Systemorganizations org WHERE a.Userid=org.Organizationowner FOR JSON PATH) AS Systemorganizations,
				(SELECT Socialsettingid,Socialowner,Socialpagename,Appid,Appsecret,Useraccesstoken,Pageaccesstoken,Pageid,Pagetype,Extra,Extra1,Extra2,Extra3,Extra4,Extra5,Extra6,Extra7,Extra8,Extra9,Extra10,Createdby,Modifiedby,Datecreated,Datemodified FROM Socialmediasettings WHERE Socialmediasettings.Socialowner=a.Userid FOR JSON PATH) AS Systemusersocials,
				(SELECT sysblog.Blogid,sysblog.Blogcategoryid,sysblogcat.Blogcategoryname,sysblog.Blogname,sysblog.Blogcontent,sysblog.Summary,sysblog.Blogprimaryimageurl,sysblog.Blogtags,sysblog.Blogowner,blogowner.Firstname+' '+blogowner.Lastname AS Blogownername,sysblog.IsPublished,sysblog.Blogstatus,sysblog.Createdby,createdby.Firstname+' '+createdby.Lastname AS Createdbyname,sysblog.Modifiedby,modifiedby.Firstname+' '+modifiedby.Lastname AS Modifiedbyname,sysblog.Datecreated,sysblog.Datemodified,
				(SELECT systblogpara.Blogparagraphid,systblogpara.Blogid,systblogpara.Blogparagraphcontent,systblogpara.Blogparagraphimageurl,systblogpara.Createdby,systblogpara.Modifiedby,systblogpara.Datecreated,systblogpara.Datemodified FROM Systemblogparagraphs systblogpara  WHERE sysblog.Blogid=systblogpara.Blogid FOR JSON PATH) AS Systemblogparagraph
				 FROM Systemblogs sysblog
				 INNER JOIN Systemblogcategories sysblogcat ON sysblog.Blogcategoryid  = sysblogcat.Blogcategoryid
				 INNER JOIN Systemstaffs blogowner ON sysblog.Blogowner =blogowner.Userid
				 INNER JOIN Systemstaffs createdby ON sysblog.Createdby =createdby.Userid
				 INNER JOIN Systemstaffs modifiedby ON sysblog.Modifiedby =modifiedby.Userid
				 WHERE a.UserId=sysblog.Blogowner
				 FOR JSON PATH
				) AS Systemuserblogs,
				(
				   SELECT 
					systemjob.JobId,systemjob.EmployerId,Systemorg.Organizationname AS Employername,Systemorg.Organizationlogo,systemjob.Title,systemjob.Jobdescription,
					systemjob.Jobfunctionid,jobfunction.Functionname,systemjob.Jobindustryid,jobindustry.Jobindustryname,systemjob.Joblocationid,joblocation.Locationname,
					systemjob.Jobtypeid,jobtype.Jobtypename,systemjob.Jobexperienceid,jobexperience.Experiencename,systemjob.Jobsalaryrange,systemjob.Deadline,systemjob.Jobstatus,
					systemjob.Dateposted,systemjob.Joburl,systemjob.Easyapply,systemjob.Hasatest,systemjob.Approved,systemjob.Ispublished,systemjob.Datecreated
					FROM Systemjobs systemjob
					INNER JOIN Jobfunctions jobfunction ON systemjob.Jobfunctionid=jobfunction.Jobfunctionid
					INNER JOIN Jobindustries jobindustry ON systemjob.Jobindustryid= jobindustry.Jobindustryid
					INNER JOIN Joblocations joblocation ON systemjob.Joblocationid=joblocation.Joblocationid
					INNER JOIN Jobtypes jobtype ON systemjob.Jobtypeid=jobtype.Jobtypeid
					INNER JOIN Jobexperiences jobexperience ON systemjob.Jobexperienceid=jobexperience.Jobexperienceid
					INNER JOIN Systemorganizations Systemorg ON systemjob.EmployerId=Systemorg.Organizationid
				    WHERE a.UserId= Systemorg.Organizationowner FOR JSON PATH )Systemjobs,
				(SELECT TOP 4 Sysuserlog.Logid,Sysuserlog.Userid,Sysuserlog.Logaction,Sysuserlog.Browser,Sysuserlog.Ipaddress,Sysuserlog.Loyaltyreward,Sysuserlog.Loyaltystatus,Sysuserlog.Logactionexittime,Sysuserlog.Datecreated FROM Systemuserlogs Sysuserlog  WHERE Sysuserlog.Userid=a.Userid ORDER BY Sysuserlog.Datecreated DESC  FOR JSON PATH) AS Systemuserlogs,
				(SELECT Sysjobapplication.Jobapplicationid,Sysjobapplication.Userid,Sysjobapplication.Jobid,Sysjob.Title,Sysjobapplication.Coverletter,Sysjobapplication.Applicationstatus,Sysjobapplication.Datecreated FROM Systemjobapplications Sysjobapplication INNER JOIN Systemjobs Sysjob ON Sysjobapplication.Jobid=Sysjob.JobId  WHERE Sysjobapplication.Userid=a.Userid ORDER BY Sysjobapplication.Datecreated DESC  FOR JSON PATH) AS Systemjobapplications
				FROM SystemStaffs a 
				WHERE a.UserId=@UserId
				FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			  )
	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;
           SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@UserProfileData AS UserProfileData  
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
