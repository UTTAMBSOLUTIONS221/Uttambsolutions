CREATE PROCEDURE [dbo].[Usp_Registersystemroledata]
@JsonObjectdata VARCHAR(MAX)
AS
BEGIN
   BEGIN
	DECLARE @RespStat int = 0,
			@RespMsg varchar(150) = '',
			@RoleId BIGINT;
		  
	BEGIN
		BEGIN TRY	
		--Validate

		BEGIN TRANSACTION;
		 DECLARE @Systemrolesdata TABLE (RoleId BIGINT);
			MERGE INTO Systemroles AS target
			USING (
			SELECT  Roleid,Rolename,RoleDescription,Tenantid,Isdefault,Isactive,Isdeleted,Createdby,Modifiedby,Datemodified,Datecreated
			FROM OPENJSON(@JsonObjectdata)
			WITH (Roleid BIGINT '$.RoleId',Rolename VARCHAR(40) '$.RoleName',RoleDescription VARCHAR(100) '$.RoleDescription',Tenantid BIGINT '$.TenantId',Isdefault BIT '$.IsDefault',Isactive BIT '$.IsActive',Isdeleted BIT '$.IsDeleted',Createdby BIGINT '$.CreatedBy',Modifiedby BIGINT '$.ModifiedBy',Datemodified DATETIME2 '$.DateModified',Datecreated DATETIME2 '$.DateCreated'
			)) AS source
			ON target.Roleid = source.Roleid
			WHEN MATCHED THEN
			UPDATE SET target.Rolename = source.Rolename,target.RoleDescription = source.RoleDescription WHEN NOT MATCHED BY TARGET THEN
			INSERT (Rolename,RoleDescription,Tenantid,Isdefault,Isactive,Isdeleted,Createdby,Modifiedby,Datemodified,Datecreated)
			VALUES (source.Rolename,source.RoleDescription,source.Tenantid,source.Isdefault,source.Isactive,source.Isdeleted,source.Createdby,source.Modifiedby,source.Datemodified,source.Datecreated)
		    OUTPUT inserted.RoleId INTO @Systemrolesdata;

        -- Set @Jobid to the new Jobid if an insert occurred
          SET @RoleId = (SELECT TOP 1 RoleId FROM @Systemrolesdata);

			
			INSERT INTO Systemroleperms (RoleId, PermissionId)
			SELECT ISNULL(@RoleId, JSON_VALUE(@JsonObjectdata, '$.Roleid')), p.PermissionId 
			FROM OPENJSON(@JsonObjectdata, '$.Permissions') 
			WITH ( PermissionId BIGINT '$.PermissionId' ) AS p;
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
