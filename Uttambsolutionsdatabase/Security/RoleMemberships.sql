ALTER ROLE [db_securityadmin] ADD MEMBER [uttambadmin];


GO
ALTER ROLE [db_owner] ADD MEMBER [uttambadmin];


GO
ALTER ROLE [db_ddladmin] ADD MEMBER [uttambadmin];


GO
ALTER ROLE [db_datawriter] ADD MEMBER [uttambadmin];


GO
ALTER ROLE [db_datareader] ADD MEMBER [uttambadmin];


GO
ALTER ROLE [db_backupoperator] ADD MEMBER [uttambadmin];


GO
ALTER ROLE [db_accessadmin] ADD MEMBER [uttambadmin];

