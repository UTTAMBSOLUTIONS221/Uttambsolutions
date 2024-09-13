CREATE PROCEDURE [dbo].[Usp_Getalltenantvacatingrequestsdata]
	@Systempropertyvacatingrequestdata VARCHAR(MAX)  OUTPUT
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
		  SET @Systempropertyvacatingrequestdata = 
		    (SELECT(SELECT Housevacatingrequest.Vacatingrequestid,Housevacatingrequest.Systempropertyhousetenantid,Systemtenant.Firstname+' '+Systemtenant.Lastname AS Systemtenantname,Houseroom.Systempropertyhousesizename,Housevacatingrequest.Systempropertyhouseroomid,Housevacatingrequest.Plannedvacatingdate,Housevacatingrequest.Expectedvacatingdate,Housevacatingrequest.Vacatingreason,Housevacatingrequest.Vacatingstatus,Housevacatingrequest.Approvedby,Housevacatingrequest.Datecreated,Housevacatingrequest.Dateapproved
			FROM Systempropertyhousevacatingrequests Housevacatingrequest
			INNER JOIN Systemstaffs Systemtenant ON Housevacatingrequest.Systempropertyhousetenantid=Systemtenant.Userid
			INNER JOIN Systemstaffdesignations Staffdesignation ON Systemtenant.Userid=Staffdesignation.Systemstaffid
			INNER JOIN Systempropertyhouserooms Houseroom ON Housevacatingrequest.Systempropertyhouseroomid=Houseroom.Systempropertyhouseroomid
			--INNER JOIN Systempropertyhouseroomstenant Houseroomstenant ON Housevacatingrequest.Systempropertyhousetenantid=Houseroomstenant.Systempropertyhousetenantid
			INNER JOIN Systempropertyhouses Systempropertyhouse ON Houseroom.Systempropertyhouseid=Systempropertyhouse.Propertyhouseid
			FOR JSON PATH
			) AS Data
			FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			);
	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;
           SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systempropertyvacatingrequestdata AS Systempropertyvacatingrequestdata  
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