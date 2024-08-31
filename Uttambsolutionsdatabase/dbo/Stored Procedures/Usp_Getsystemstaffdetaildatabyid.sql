CREATE PROCEDURE [dbo].[Usp_Getsystemstaffdetaildatabyid]
    @Staffid BIGINT,
	@Systemstaffdetaildata VARCHAR(MAX)  OUTPUT
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
		SET @Systemstaffdetaildata= 
		  (SELECT(SELECT Systemstaff.Userid,Systemstaff.Firstname+' '+Systemstaff.Lastname AS Fullname,Systemstaff.Phonenumber,Systemstaff.Loginstatus,Systemstaffsaccount.Accountid,ISNULL(Systemstaffsaccount.Accountnumber,0) AS Accountnumber,
		  CASE WHEN DESG.Staffdesignation='Owner' THEN (SELECT SUM(SUB.Propertyhousesubscriptionamount) FROM Propertyhousesubscriptions SUB WHERE SUB.Propertyownerid=Systemstaff.Userid) WHEN DESG.Staffdesignation='Tenant' THEN 50 WHEN DESG.Staffdesignation='Agent' THEN 2000  ELSE 50  END AS Subscriptionamount,
		  (SELECT Systemaccountverifaction.Verificationid,Systemaccountverifaction.Verificationname,Systemaccountverifaction.Verificationtype,Systemaccountverifaction.Verificationshortcode,Systemaccountverifaction.Accountnumber,Systemaccountverifaction.Isactive FROM Systemaccountverifaction Systemaccountverifaction WHERE Systemaccountverifaction.Isactive=1 FOR JSON PATH) AS AccountVerificationBanks
		 FROM Systemstaffs Systemstaff 
		 LEFT JOIN Systemstaffsaccount Systemstaffsaccount ON Systemstaff.Userid=Systemstaffsaccount.Userid
		 LEFT JOIN Systemstaffdesignations DESG ON Systemstaff.Userid=DESG.Systemstaffid
		 WHERE Systemstaff.UserId=@Staffid
		 FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
		 ) AS Data
		FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
	     );
	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;
           SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systemstaffdetaildata AS Systemstaffdetaildata
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