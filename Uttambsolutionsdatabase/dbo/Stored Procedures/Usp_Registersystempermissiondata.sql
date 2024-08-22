CREATE PROCEDURE [dbo].[Usp_Registersystempermissiondata]
    @JsonObjectdata NVARCHAR(MAX)
AS
BEGIN
    DECLARE 
        @RespStat int = 0,
        @RespMsg varchar(150) = '',
        @PermissionId BIGINT;

    BEGIN TRY
        -- Validate

        BEGIN TRANSACTION;
	    DECLARE @Systempermissionsdata TABLE (PermissionId BIGINT);
        MERGE Systempermissions AS target
        USING (SELECT 
                   JSON_VALUE(@JsonObjectdata, '$.PermissionId') AS PermissionId, 
                   JSON_VALUE(@JsonObjectdata, '$.Permissionname') AS Permissionname,
                   JSON_VALUE(@JsonObjectdata, '$.Isadmin') AS Isadmin
               ) AS source
        ON (target.PermissionId = source.PermissionId)
        WHEN MATCHED AND source.PermissionId != 0 THEN
            UPDATE SET 
                target.Permissionname = source.Permissionname,
                target.Isadmin = source.Isadmin
        WHEN NOT MATCHED AND source.PermissionId = 0 THEN
            INSERT (Permissionname, Isactive, Isdeleted, Module, Isadmin)
            VALUES (source.Permissionname, 1, 0, 1, source.Isadmin)
		    OUTPUT inserted.PermissionId INTO @Systempermissionsdata;

        -- Set @Jobid to the new Jobid if an insert occurred
          SET @PermissionId = (SELECT TOP 1 PermissionId FROM @Systempermissionsdata);


        -- Check if a new permission was inserted
        IF @PermissionId IS NULL
            SET @PermissionId = JSON_VALUE(@JsonObjectdata, '$.PermissionId');

        IF JSON_VALUE(@JsonObjectdata, '$.PermissionId') = 0
        BEGIN
            IF JSON_VALUE(@JsonObjectdata, '$.Isadmin') = 'true'
            BEGIN
                INSERT INTO Systemroleperms (Roleid, PermissionId)
                SELECT Roleid, @PermissionId
                FROM SystemRoles 
                WHERE Rolename = 'Super Admin';
            END
            ELSE
            BEGIN
                INSERT INTO Systemroleperms (Roleid, PermissionId)
                SELECT Roleid, @PermissionId
                FROM SystemRoles 
                WHERE Rolename IN ('Super Admin', 'System Super Admin');
            END
            SET @RespMsg = 'Permission Saved Successfully.';
        END
        ELSE
        BEGIN
            SET @RespMsg = 'Permission Updated Successfully.';
        END
        
        SET @RespStat = 0;
        COMMIT TRANSACTION;

        SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT 'Error ' + ERROR_MESSAGE();
        SELECT 2 AS RespStatus, '0 - Error(s) Occurred: ' + ERROR_MESSAGE() AS RespMessage;
    END CATCH

    RETURN; 
END;
