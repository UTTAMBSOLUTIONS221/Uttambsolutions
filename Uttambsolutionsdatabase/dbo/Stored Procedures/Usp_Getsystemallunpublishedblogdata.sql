CREATE PROCEDURE [dbo].[Usp_Getsystemallunpublishedblogdata]
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		SELECT sysblog.Blogid,sysblog.Blogcategoryid,sysblogcat.Blogcategoryname,sysblog.Blogname,sysblog.Blogcontent,sysblog.Summary,sysblog.Blogprimaryimageurl,sysblog.Blogtags,sysblog.Blogowner,blogowner.Firstname+' '+blogowner.Lastname AS Blogownername,
		sysblog.IsPublished,sysblog.Blogstatus,sysblog.Createdby,createdby.Firstname+' '+createdby.Lastname AS Createdbyname,sysblog.Modifiedby,modifiedby.Firstname+' '+modifiedby.Lastname AS Modifiedbyname,sysblog.Datecreated,sysblog.Datemodified
		FROM Systemblogs sysblog
		INNER JOIN Systemblogcategories sysblogcat ON sysblog.Blogcategoryid  = sysblogcat.Blogcategoryid
		INNER JOIN Systemstaffs blogowner ON sysblog.Blogowner =blogowner.Userid
		INNER JOIN Systemstaffs createdby ON sysblog.Createdby =createdby.Userid
		INNER JOIN Systemstaffs modifiedby ON sysblog.Modifiedby =modifiedby.Userid
		WHERE sysblog.IsPublished=0 AND sysblog.Blogstatus=0

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
