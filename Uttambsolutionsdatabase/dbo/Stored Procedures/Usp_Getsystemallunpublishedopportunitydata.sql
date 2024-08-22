CREATE PROCEDURE [dbo].[Usp_Getsystemallunpublishedopportunitydata]
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
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
		WHERE systemjob.IsPublished=0 AND systemjob.Jobstatus='Open'

	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage;

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
