CREATE PROCEDURE [dbo].[Usp_Getsystemblogdata]
@Systemblogdata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		SET @Systemblogdata= 
		  (SELECT(SELECT sysblog.Blogid,sysblog.Blogcategoryid,sysblogcat.Blogcategoryname,sysblog.Blogname,sysblog.Blogcontent,sysblog.Summary,sysblog.Blogprimaryimageurl,sysblog.Blogtags,sysblog.Blogowner,blogowner.Firstname+' '+blogowner.Lastname AS Blogownername,
			sysblog.IsPublished,sysblog.Blogstatus,sysblog.Createdby,createdby.Firstname+' '+createdby.Lastname AS Createdbyname,sysblog.Modifiedby,modifiedby.Firstname+' '+modifiedby.Lastname AS Modifiedbyname,sysblog.Datecreated,sysblog.Datemodified,
			(SELECT systblogpara.Blogparagraphid,systblogpara.Blogid,systblogpara.Blogparagraphcontent,systblogpara.Blogparagraphimageurl,systblogpara.Createdby,systblogpara.Modifiedby,systblogpara.Datecreated,systblogpara.Datemodified FROM Systemblogparagraphs systblogpara WHERE sysblog.Blogid=systblogpara.Blogid FOR JSON PATH
			) AS Systemblogparagraph
			FROM Systemblogs sysblog
			INNER JOIN Systemblogcategories sysblogcat ON sysblog.Blogcategoryid  = sysblogcat.Blogcategoryid
			INNER JOIN Systemstaffs blogowner ON sysblog.Blogowner =blogowner.Userid
			INNER JOIN Systemstaffs createdby ON sysblog.Createdby =createdby.Userid
			INNER JOIN Systemstaffs modifiedby ON sysblog.Modifiedby =modifiedby.Userid
			WHERE sysblog.Blogstatus=0
			ORDER BY sysblog.DateCreated DESC
			FOR JSON PATH
			)Systemblogs
			FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			);

	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systemblogdata AS Systemblogdata;

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
