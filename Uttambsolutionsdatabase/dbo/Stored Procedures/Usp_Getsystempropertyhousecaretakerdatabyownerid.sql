CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhousecaretakerdatabyownerid]
    @Ownerid BIGINT,
	@Systempropertyhousecaretakerdata VARCHAR(MAX)  OUTPUT
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

		SET @Systempropertyhousecaretakerdata= 
		  (SELECT(SELECT Systemstaff.Userid,Caretaker.Propertyhouseid,Systemstaff.Firstname,Systemstaff.Lastname,Systemstaff.Firstname+' '+Systemstaff.Lastname AS Fullname,HSC.Propertyhousename,Systemstaff.Phonenumber,Systemstaffdesignation.Staffdesignation,Systemstaff.Username,Systemstaff.Emailaddress,Systemstaff.Genderid,Systemstaff.Maritalstatusid,Systemstaff.Roleid,Systemstaff.Passharsh,Systemstaff.Passwords,Systemstaff.Isactive,Systemstaff.Isdeleted,Systemstaff.Isdefault,Systemstaff.Loginstatus,Systemstaff.Passwordresetdate,Systemstaff.Parentid,Systemstaff.Userprofileimageurl,Systemstaff.Usercurriculumvitae,Systemstaff.Idnumber,CASE WHEN Systemstaff.Idnumber IS NULL THEN 0 ELSE 1 END AS Columnreadonly,Systemstaff.Updateprofile,Systemstaff.Extra,Systemstaff.Extra1,Systemstaff.Extra2,Systemstaff.Extra3,Systemstaff.Extra4,Systemstaff.Extra5,Systemstaff.Createdby,Systemstaff.Modifiedby,ISNULL(Systemstaffsaccount.Accountnumber,0) AS Accountnumber,1 AS Subscriptionamount,Systemstaff.Lastlogin,Systemstaffkin.Kinname,Systemstaffkin.Kinphonenumber,Systemstaffkin.Kinrelationshipid,Systemstaff.Datemodified,Systemstaff.Datecreated 
			FROM Systemstaffs Systemstaff 
			LEFT JOIN Systemstaffsaccount Systemstaffsaccount ON Systemstaff.Userid=Systemstaffsaccount.Userid
			LEFT JOIN Systemstaffdesignations Systemstaffdesignation ON Systemstaff.Userid=Systemstaffdesignation.Systemstaffid
			LEFT JOIN Systemstaffkins Systemstaffkin ON Systemstaff.Userid=Systemstaffkin.Userid
			LEFT JOIN Systemcaretakerhouse Caretaker ON Systemstaff.Userid=Caretaker.Caretakerid
			LEFT JOIN Systempropertyhouses HSC ON Caretaker.Propertyhouseid=HSC.Propertyhouseid
			WHERE Systemstaff.Parentid=@Ownerid
		 	FOR JSON PATH
		 ) AS Data
		 FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
		 )
	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;
           SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systempropertyhousecaretakerdata AS Systempropertyhousecaretakerdata;
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