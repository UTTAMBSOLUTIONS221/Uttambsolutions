CREATE PROCEDURE [dbo].[Usp_Getsystemstaffdatabyidnumber]
    @Idnumber INT,
	@Systempropertyhouseroomtenantdata VARCHAR(MAX)  OUTPUT
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
		 SET @Systempropertyhouseroomtenantdata= 
		  (SELECT(SELECT a.Userid,a.Firstname+' '+a.Lastname AS Fullname,a.Phonenumber,a.Loginstatus,a.Idnumber,
		   (SELECT Systempropertyhousetenantsroom.Systempropertyhousetenantid,Systempropertyhousetenant.Idnumber,Systempropertyhousetenant.Firstname+' '+Systempropertyhousetenant.Lastname AS Tenantname,Systempropertyhouse.Propertyhousename,'' AS Propertyprimaryimage,Systemhousesize.Systemhousesizename +''+Systempropertyhouseroom.Systempropertyhousesizename AS Systempropertyhousesizename,
			Systempropertyhousetenantsroom.Isoccupant,CASE WHEN Systempropertyhousetenantsroom.Occupationalstatus=0 THEN 'Vacated' WHEN Systempropertyhousetenantsroom.Occupationalstatus= 1 THEN 'Vacating' ELSE 'Occupant' END AS Occupationalstatus,Systempropertyhousetenantsroom.Datecreated,Systempropertyhousetenantsroom.Datemodified
			FROM Systempropertyhouseroomstenant Systempropertyhousetenantsroom
			INNER JOIN Systempropertyhouserooms Systempropertyhouseroom ON Systempropertyhousetenantsroom.Systempropertyhouseroomid=Systempropertyhouseroom.Systempropertyhouseroomid
			INNER JOIN Systempropertyhousesizes Systempropertyhousesize ON Systempropertyhouseroom.Systempropertyhousesizeid=Systempropertyhousesize.Systemhousesizeid
			INNER JOIN Systemhousesizes  Systemhousesize ON Systempropertyhousesize.Systemhousesizeid=Systemhousesize.Systemhousesizeid
			INNER JOIN Systempropertyhouses  Systempropertyhouse ON Systempropertyhousesize.Propertyhouseid=Systempropertyhouse.Propertyhouseid
			INNER JOIN Systemstaffs Systempropertyhousetenant ON Systempropertyhousetenantsroom.Systempropertyhousetenantid= Systempropertyhousetenant.Userid
			WHERE Systempropertyhousetenantsroom.Systempropertyhousetenantid=a.Userid
			FOR JSON PATH) AS Tenantroomhistory
		    FROM SystemStaffs a WHERE a.Idnumber=@Idnumber
		 	FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
		 ) AS Data
		 FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
		 )
	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;
           SELECT  @RespStat as RespStatus, @RespMsg as RespMessage 
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
