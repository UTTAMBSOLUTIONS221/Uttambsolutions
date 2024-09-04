CREATE PROCEDURE [dbo].[Usp_Getsystemagentpropertyhouseroomtenantsdata]
@Agentid INT,
@Systempropertyhouseroomtenantsdata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		SET @Systempropertyhouseroomtenantsdata= 
		      (SELECT(SELECT Systemtenant.Systempropertyhousetenantentryid,Systemtenant.Systempropertyhousetenantid,Systempropertyhousetenant.Idnumber,Systempropertyhousetenant.Firstname+' '+Systempropertyhousetenant.Lastname AS Tenantname,Systempropertyhouse.Propertyhousename,'' AS Propertyprimaryimage,Systemhousesize.Systemhousesizename +'-'+Systempropertyhouseroom.Systempropertyhousesizename AS Systempropertyhousesizename,
			  Systemtenant.Systempropertyhouseroomid,Systemtenant.Isoccupant,
			  CASE WHEN Systemtenant.Occupationalstatus=0 THEN 'Vacated' WHEN Systemtenant.Occupationalstatus= 1 THEN 'Vacating' ELSE 'Occupant' END AS Occupationalstatus,Systemtenant.Createdby,Systemtenant.Modifiedby,Systemtenant.Datecreated,Systemtenant.Datemodified,0 AS Walletbalance
			  FROM Systempropertyhouseroomstenant Systemtenant
			  INNER JOIN Systempropertyhouserooms Systempropertyhouseroom ON Systemtenant.Systempropertyhouseroomid =Systempropertyhouseroom.Systempropertyhouseroomid
			  INNER JOIN Systempropertyhousesizes Systempropertyhousesize ON Systempropertyhouseroom.Systempropertyhousesizeid=Systempropertyhousesize.Systempropertyhousesizeid
			  INNER JOIN Systemhousesizes Systemhousesize ON Systempropertyhousesize.Systemhousesizeid=Systemhousesize.Systemhousesizeid
			  INNER JOIN Systempropertyhouses Systempropertyhouse ON Systempropertyhouseroom.Systempropertyhouseid=Systempropertyhouse.Propertyhouseid
			  INNER JOIN Systemstaffdesignations  Systemstaffdesignation ON Systemtenant.Systempropertyhousetenantid=Systemstaffdesignation.Systemstaffid
			  INNER JOIN Systemstaffs Systempropertyhousetenant ON Systemtenant.Systempropertyhousetenantid= Systempropertyhousetenant.Userid
			  WHERE Systemstaffdesignation.Staffdesignation='Tenant' AND Systemtenant.Occupationalstatus IN(1,2) AND Systempropertyhouse.Propertyhouseposter=@Agentid
		      FOR JSON PATH
			) AS Data
		   FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
	     );

	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systempropertyhouseroomtenantsdata AS Systempropertyhouseroomtenantsdata;

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