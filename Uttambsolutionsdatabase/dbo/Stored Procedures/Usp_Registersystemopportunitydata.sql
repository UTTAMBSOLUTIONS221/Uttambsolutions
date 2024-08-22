CREATE PROCEDURE [dbo].[Usp_Registersystemopportunitydata]
@JsonObjectdata VARCHAR(MAX)
AS
BEGIN
   BEGIN
	DECLARE @RespStat int = 0,
			@RespMsg varchar(150) = '',
			@Jobid BIGINT = NULL,
			@JobKeyResponsibilityId BIGINT = NULL;
		  
	BEGIN
		BEGIN TRY	
		--Validate

		BEGIN TRANSACTION;
		  DECLARE @Systemjobdata TABLE (Jobid BIGINT);
		  DECLARE @Systemjobkeyresponsibilitydata TABLE (JobKeyResponsibilityId BIGINT);
			MERGE INTO Systemjobs AS target
			USING (SELECT JobId,EmployerId,Title,Jobdescription,Jobfunctionid,Jobindustryid,Joblocationid,Jobtypeid,Jobexperienceid,
			Jobsalaryrange,Deadline,Jobstatus,Dateposted,Joburl,Easyapply,Hasatest,Isfeatured,Approved,Ispublished,Jobreportto,Jobimageburl,Jobhowtoapply,Createdby,Datecreated
			FROM OPENJSON(@JsonObjectdata)
			WITH (JobId BIGINT '$.JobId',EmployerId INT '$.EmployerId',Title VARCHAR(100) '$.Title',Jobdescription VARCHAR(MAX) '$.JobDescription',
			Jobfunctionid INT '$.JobFunctionId',Jobindustryid INT '$.JobIndustryId',Joblocationid INT '$.JobLocationId',Jobtypeid INT '$.JobTypeId',
			Jobexperienceid INT '$.JobExperienceId',Jobsalaryrange VARCHAR(100) '$.JobSalaryRange',Deadline DATETIME2 '$.Deadline',Jobstatus VARCHAR(50) '$.JobStatus',
			Dateposted DATETIME2 '$.DatePosted',Joburl VARCHAR(500) '$.JobUrl',Easyapply BIT '$.EasyApply',Hasatest BIT '$.HasTest',Isfeatured BIT '$.IsFeatured',
			Approved BIT '$.Approved',Ispublished BIT '$.IsPublished',Jobreportto VARCHAR(200) '$.Jobreportto',Jobimageburl VARCHAR(240) '$.Jobimageburl',Jobhowtoapply VARCHAR(1000) '$.Jobhowtoapply',Createdby BIGINT '$.CreatedBy',Datecreated DATETIME2 '$.DateCreated')
			) AS source
			ON target.JobId = source.JobId
			WHEN MATCHED THEN
			UPDATE SET target.EmployerId = source.EmployerId,target.Title = source.Title,target.Jobdescription = source.Jobdescription,target.Jobfunctionid = source.Jobfunctionid,
			target.Jobindustryid = source.Jobindustryid,target.Joblocationid = source.Joblocationid,target.Jobtypeid = source.Jobtypeid,target.Jobexperienceid = source.Jobexperienceid,
			target.Jobsalaryrange = source.Jobsalaryrange,target.Deadline = source.Deadline,target.Jobstatus = source.Jobstatus,target.Dateposted = source.Dateposted,target.Joburl = source.Joburl,
			target.Easyapply = source.Easyapply,target.Hasatest = source.Hasatest,target.Isfeatured = source.Isfeatured,target.Approved = source.Approved,target.Ispublished = source.Ispublished,
			target.Jobreportto = source.Jobreportto,target.Jobimageburl = source.Jobimageburl,target.Jobhowtoapply = source.Jobhowtoapply,target.Datecreated = source.Datecreated
			WHEN NOT MATCHED BY TARGET THEN
			INSERT (EmployerId,Title,Jobdescription,Jobfunctionid,Jobindustryid,Joblocationid,Jobtypeid,Jobexperienceid,Jobsalaryrange,Deadline,Jobstatus,Dateposted,
			Joburl,Easyapply,Hasatest,Isfeatured,Approved,Ispublished,Jobreportto,Jobimageburl,Jobhowtoapply,Createdby,Datecreated)
			VALUES (source.EmployerId,source.Title,source.Jobdescription,source.Jobfunctionid,source.Jobindustryid,source.Joblocationid,source.Jobtypeid,source.Jobexperienceid,
			source.Jobsalaryrange,source.Deadline,source.Jobstatus,source.Dateposted,source.Joburl,source.Easyapply,source.Hasatest,source.Isfeatured,source.Approved,
			source.Ispublished,source.Jobreportto,source.Jobimageburl,source.Jobhowtoapply,source.Createdby,source.Datecreated)
			OUTPUT inserted.JobId INTO @Systemjobdata;

        -- Set @Jobid to the new Jobid if an insert occurred
          SET @Jobid = (SELECT TOP 1 Jobid FROM @Systemjobdata);

			-- MERGE for Systemjobfunctions table
			MERGE INTO Systemjobfunction AS target
			USING (
				SELECT Jobfunctionid,ISNULL(@Jobid, Jobid) AS Jobid,Jobfunction,DateCreated 
				FROM OPENJSON(@JsonObjectdata, '$.Systemjobfunction')
                WITH (Jobfunctionid INT '$.Jobfunctionid',Jobid BIGINT '$.JobId',Jobfunction VARCHAR(1000) '$.Jobfunction',DateCreated DATETIME2 '$.DateCreated')
				WHERE Jobfunction IS NOT NULL AND Jobfunction <> ''
			  ) AS source
			ON target.Jobfunctionid = source.Jobfunctionid
			WHEN MATCHED THEN
				UPDATE SET target.Jobfunction = source.Jobfunction
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (JobId,Jobfunction,DateCreated)
				VALUES (source.JobId,source.Jobfunction,source.DateCreated);

			-- MERGE for Systemjobqualification table
			MERGE INTO Systemjobqualification AS target
			USING (
				SELECT Jobqualificationid,ISNULL(@Jobid, Jobid) AS Jobid,Jobqualification,DateCreated 
				FROM OPENJSON(@JsonObjectdata, '$.Systemjobqualification')
				WITH (Jobqualificationid INT '$.Jobqualificationid',Jobid BIGINT '$.JobId',Jobqualification VARCHAR(400) '$.Jobqualification',DateCreated DATETIME2 '$.DateCreated')
				WHERE Jobqualification IS NOT NULL AND Jobqualification <> ''
				) AS source
			ON target.Jobqualificationid = source.Jobqualificationid
			WHEN MATCHED THEN
				UPDATE SET target.Jobqualification = source.Jobqualification
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (JobId,Jobqualification,DateCreated)
				VALUES (source.JobId,source.Jobqualification,source.DateCreated);
				
			-- MERGE for Systemjobskill table
			MERGE INTO Systemjobskill AS target
			USING (
				SELECT Jobskillid,ISNULL(@Jobid, Jobid) AS Jobid,Jobskill,DateCreated 
				FROM OPENJSON(@JsonObjectdata, '$.Systemjobskill')
				WITH (Jobskillid INT '$.Jobskillid',Jobid BIGINT '$.JobId',Jobskill VARCHAR(400) '$.Jobskill',DateCreated DATETIME2 '$.DateCreated')
				WHERE Jobskill IS NOT NULL AND Jobskill <> ''
				) AS source
			ON target.Jobskillid = source.Jobskillid
			WHEN MATCHED THEN
				UPDATE SET target.Jobskill = source.Jobskill
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (JobId,Jobskill,DateCreated)
				VALUES (source.JobId,source.Jobskill,source.DateCreated);

				-- MERGE for Systemjobbenefit table
			MERGE INTO Systemjobbenefit AS target
			USING (
				SELECT Jobbenefitid,ISNULL(@Jobid, Jobid) AS Jobid,Jobbenefit,DateCreated 
				FROM OPENJSON(@JsonObjectdata, '$.Systemjobbenefit')
				WITH (Jobbenefitid INT '$.Jobbenefitid',Jobid BIGINT '$.JobId',Jobbenefit VARCHAR(400) '$.Jobbenefit',DateCreated DATETIME2 '$.DateCreated')
				WHERE Jobbenefit IS NOT NULL AND Jobbenefit <> ''
				) AS source
			ON target.Jobbenefitid = source.Jobbenefitid
			WHEN MATCHED THEN
				UPDATE SET target.Jobbenefit = source.Jobbenefit
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (JobId,Jobbenefit,DateCreated)
				VALUES (source.JobId,source.Jobbenefit,source.DateCreated);
 
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
