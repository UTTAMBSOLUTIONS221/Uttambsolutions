CREATE PROCEDURE [dbo].[Usp_Getsystemallopportunitydata]
@Systemjobdata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		SET @Systemjobdata= 
		  (SELECT
		  (
		        SELECT 
		            systemjob.JobId,systemjob.EmployerId,Systemorg.Organizationname AS Employername,Systemorg.Organizationlogo,systemjob.Title,systemjob.Jobdescription,
					systemjob.Jobfunctionid,jobfunction.Functionname,systemjob.Jobindustryid,jobindustry.Jobindustryname,systemjob.Joblocationid,joblocation.Locationname,
					systemjob.Jobtypeid,jobtype.Jobtypename,systemjob.Jobexperienceid,jobexperience.Experiencename,systemjob.Jobsalaryrange,systemjob.Deadline,systemjob.Jobstatus,
					systemjob.Dateposted,systemjob.Joburl,systemjob.Easyapply,systemjob.Hasatest,systemjob.Isfeatured,systemjob.Approved,systemjob.Ispublished,systemjob.Jobreportto,
					systemjob.Jobimageburl,systemjob.Jobhowtoapply,systemjob.Datecreated,
					(SELECT Sysjobfunction.Jobfunctionid,Sysjobfunction.Jobid,Sysjobfunction.Jobfunction,Sysjobfunction.Datecreated FROM Systemjobfunction Sysjobfunction WHERE systemjob.Jobid=Sysjobfunction.JobId FOR JSON PATH) AS Systemjobfunction,
					(SELECT Sysjobqualification.Jobqualificationid,Sysjobqualification.Jobid,Sysjobqualification.Jobqualification,Sysjobqualification.Datecreated FROM Systemjobqualification Sysjobqualification WHERE systemjob.Jobid=Sysjobqualification.JobId  FOR JSON PATH) AS Systemjobqualification,
					(SELECT Sysjobskill.Jobskillid,Sysjobskill.Jobid,Sysjobskill.Jobskill,Sysjobskill.Datecreated FROM Systemjobskill Sysjobskill WHERE systemjob.Jobid=Sysjobskill.JobId FOR JSON PATH) AS Systemjobskill,
					(SELECT Sysjobbenefit.Jobbenefitid,Sysjobbenefit.Jobid,Sysjobbenefit.Jobbenefit,Sysjobbenefit.Datecreated FROM Systemjobbenefit Sysjobbenefit WHERE systemjob.Jobid=Sysjobbenefit.JobId FOR JSON PATH) AS Systemjobbenefit
				FROM Systemjobs systemjob
				INNER JOIN Jobfunctions jobfunction ON systemjob.Jobfunctionid=jobfunction.Jobfunctionid
				INNER JOIN Jobindustries jobindustry ON systemjob.Jobindustryid= jobindustry.Jobindustryid
				INNER JOIN Joblocations joblocation ON systemjob.Joblocationid=joblocation.Joblocationid
				INNER JOIN Jobtypes jobtype ON systemjob.Jobtypeid=jobtype.Jobtypeid
				INNER JOIN Jobexperiences jobexperience ON systemjob.Jobexperienceid=jobexperience.Jobexperienceid
				INNER JOIN Systemorganizations Systemorg ON systemjob.EmployerId=Systemorg.Organizationid
				ORDER BY systemjob.DateCreated DESC
			FOR JSON PATH
			)Systemjobs
			FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			);

	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systemjobdata AS Systemjobdata;

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
