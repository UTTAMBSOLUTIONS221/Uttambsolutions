CREATE PROCEDURE [dbo].[Usp_Getsystemblogdatabyid]
@Blogid BIGINT,
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
		  (SELECT sysblog.Blogid,sysblog.Blogcategoryid,sysblogcat.Blogcategoryname,sysblog.Blogname,sysblog.Blogcontent,sysblog.Summary,sysblog.Blogprimaryimageurl,sysblog.Blogtags,sysblog.Blogowner,sysblog.Blogimagesource,sysblog.Blogimagename,blogowner.Firstname+' '+blogowner.Lastname AS Blogownername,
			sysblog.IsPublished,sysblog.Blogstatus,sysblog.Createdby,createdby.Firstname+' '+createdby.Lastname AS Createdbyname,sysblog.Modifiedby,modifiedby.Firstname+' '+modifiedby.Lastname AS Modifiedbyname,sysblog.Datecreated,sysblog.Datemodified,
			(
			SELECT systblogpara.Blogparagraphid,systblogpara.Blogid,systblogpara.Blogparagraphcontent,systblogpara.Blogparagraphimageurl,systblogpara.Blogparagraphimagesource,systblogpara.Blogparagraphimagename,systblogpara.Createdby,systblogpara.Modifiedby,systblogpara.Datecreated,systblogpara.Datemodified FROM Systemblogparagraphs systblogpara WHERE sysblog.Blogid=systblogpara.Blogid FOR JSON PATH
			) AS Systemblogparagraph,
			(SELECT TOP 10 relatedsysblog.Blogid,relatedsysblog.Blogcategoryid,sysblogcat.Blogcategoryname,relatedsysblog.Blogname,relatedsysblog.Blogcontent,relatedsysblog.Summary,relatedsysblog.Blogprimaryimageurl,relatedsysblog.Blogtags,relatedsysblog.Blogowner,blogowner.Firstname+' '+blogowner.Lastname AS Blogownername,
				 relatedsysblog.IsPublished,relatedsysblog.Blogstatus,relatedsysblog.Createdby,createdby.Firstname+' '+createdby.Lastname AS Createdbyname,relatedsysblog.Modifiedby,modifiedby.Firstname+' '+modifiedby.Lastname AS Modifiedbyname,relatedsysblog.Datecreated,relatedsysblog.Datemodified
				FROM Systemblogs relatedsysblog
				INNER JOIN Systemblogcategories sysblogcat ON relatedsysblog.Blogcategoryid  = sysblogcat.Blogcategoryid
				INNER JOIN Systemstaffs blogowner ON relatedsysblog.Blogowner =blogowner.Userid
				INNER JOIN Systemstaffs createdby ON relatedsysblog.Createdby =createdby.Userid
				INNER JOIN Systemstaffs modifiedby ON relatedsysblog.Modifiedby =modifiedby.Userid
				WHERE relatedsysblog.Blogstatus=0 AND sysblog.Blogid!=relatedsysblog.Blogid
				ORDER BY relatedsysblog.DateCreated DESC
				FOR JSON PATH
			   )Relatedarticles
			FROM Systemblogs sysblog
			INNER JOIN Systemblogcategories sysblogcat ON sysblog.Blogcategoryid  = sysblogcat.Blogcategoryid
			INNER JOIN Systemstaffs blogowner ON sysblog.Blogowner =blogowner.Userid
			INNER JOIN Systemstaffs createdby ON sysblog.Createdby =createdby.Userid
			INNER JOIN Systemstaffs modifiedby ON sysblog.Modifiedby =modifiedby.Userid
			WHERE sysblog.Blogid=@Blogid
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
