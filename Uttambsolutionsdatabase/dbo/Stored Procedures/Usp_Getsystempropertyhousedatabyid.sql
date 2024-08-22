CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhousedatabyid]
@Propertyhouseid BIGINT,
@Systempropertydata VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
			IF(@Propertyhouseid=0)
			 BEGIN
			  SET @Systempropertydata= 
				  (SELECT 0 AS Propertyhouseid,0 AS Isagency,0 AS Propertyhouseowner,0 AS Propertyhouseposter,'' AS Propertyhousename,0 AS Countyid,0 AS Subcountyid,0 AS Subcountywardid,'' AS Streetorlandmark,'' AS Contactdetails,0 AS Hashousedeposit,0 AS Hasagent,3 AS Propertyhousestatus,0 AS Watertypeid,0 AS Waterunitprice,0 AS Monthlycollection,0 AS Rentdueday,'' AS Extra,'' AS Extra1,'' AS Extra2,'' AS Extra3,'' AS Extra4,'' AS Extra5,'' AS Extra6,'' AS Extra7,'' AS Extra8,'' AS Extra9,'' AS Extra10,0 AS Createdby,0 AS Modifiedby,GETDATE() AS Datecreated,GETDATE() AS Datemodified,
					(
						SELECT 
							0 AS Systempropertyhousesizeid,
							0 AS Propertyhouseid,
							ISNULL(Systemhousesize.Systemhousesizeid, 0) AS Systemhousesizeid,
							ISNULL(Systemhousesize.Systemhousesizename, '') AS Systemhousesizename,
							0 AS Systempropertyhousesizeunits,
							0 AS Systempropertyhousesizerent,
							0 AS Systempropertyhousesizedeposit,
							0 AS Systempropertyhousesizewehave
						FROM 
							Systemhousesizes Systemhousesize
						FOR JSON PATH
					) AS Propertyhousesize,
					(
						SELECT 
							0 AS Systempropertybankaccountid,
							0 AS Propertyhouseid,
							ISNULL(Systemsupportedbank.Systembankid, 0) AS Systembankid,
							ISNULL(Systemsupportedbank.Systembankname+' '+ Systemsupportedbank.Systembankpaybill, '') AS Systembanknameandpaybill,
							''  AS Systempropertybankaccount,
							0 AS Systempropertyhousebankwehave
						FROM Systemsupportedbanks Systemsupportedbank
						FOR JSON PATH
					) AS Propertyhousebankingdetail,
					(
						SELECT 
							0 AS Systempropertyhousedepositfeeid,
							0 AS Propertyhouseid,
							ISNULL(Systemhousedepositfee.Housedepositfeeid, 0) AS Housedepositfeeid,
							ISNULL(Systemhousedepositfee.Housedepositfeename, '') AS Housedepositfeename,
							0 AS Systempropertyhousedepositfeeamount,
							0 AS Systempropertyhousesizedepositfeewehave
						FROM Systemhousedepositfees Systemhousedepositfee
						FOR JSON PATH
					) AS Propertyhousedepositfee,
					(
						SELECT 
							0 AS Systempropertyhousebenefitid,
							0 AS Propertyhouseid,
							ISNULL(Systemhousebenefit.Housebenefitid, 0) AS Housebenefitid,
							ISNULL(Systemhousebenefit.Housebenefitname, '') AS Housebenefitname,
							0 AS Systempropertyhousebenefitwehave
						FROM Systemhousebenefits Systemhousebenefit
						FOR JSON PATH
					) AS Propertyhousebenefit
					FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
				 );
			  END
			ELSE
			BEGIN
			  SET @Systempropertydata= 
				  (SELECT Systempropertyhouse.Propertyhouseid,Systempropertyhouse.Isagency,Systempropertyhouse.Propertyhouseowner,Systempropertyhouse.Propertyhouseposter,Systempropertyhouse.Propertyhousename,
				   Systempropertyhouse.Countyid,Systempropertyhouse.Subcountyid,Systempropertyhouse.Subcountywardid,Systempropertyhouse.Streetorlandmark,Systempropertyhouse.Contactdetails,Systempropertyhouse.Hashousedeposit,Systempropertyhouse.Rentdepositmonth,Systempropertyhouse.Vacantnoticeperiod,Systempropertyhouse.Monthlycollection,Systempropertyhouse.Hasagent,Systempropertyhouse.Propertyhousestatus,Systempropertyhouse.Watertypeid,Systempropertyhouse.Hashousewatermeter,Systempropertyhouse.Waterunitprice,Systempropertyhouse.Rentdueday,
				   Systempropertyhouse.Extra,Systempropertyhouse.Extra1,Systempropertyhouse.Extra2,Systempropertyhouse.Extra3,Systempropertyhouse.Extra4,Systempropertyhouse.Extra5,Systempropertyhouse.Extra6,Systempropertyhouse.Extra7,
				   Systempropertyhouse.Extra8,Systempropertyhouse.Extra9,Systempropertyhouse.Extra10,Systempropertyhouse.Createdby,Systempropertyhouse.Modifiedby,Systempropertyhouse.Datecreated,Systempropertyhouse.Datemodified,
					(
						SELECT 
							ISNULL(Systempropertyhousesize.Systempropertyhousesizeid, 0) AS Systempropertyhousesizeid,
							ISNULL(Systempropertyhousesize.Propertyhouseid, 0) AS Propertyhouseid,
							ISNULL(Systemhousesize.Systemhousesizeid, 0) AS Systemhousesizeid,
							ISNULL(Systemhousesize.Systemhousesizename, '') AS Systemhousesizename,
							ISNULL(Systempropertyhousesize.Systempropertyhousesizeunits, 0) AS Systempropertyhousesizeunits,
							ISNULL(Systempropertyhousesize.Systempropertyhousesizewehave, 0) AS Systempropertyhousesizewehave
						FROM Systemhousesizes Systemhousesize
						LEFT JOIN Systempropertyhousesizes Systempropertyhousesize ON Systemhousesize.Systemhousesizeid = Systempropertyhousesize.Systemhousesizeid
						WHERE Systempropertyhouse.Propertyhouseid=Systempropertyhousesize.Propertyhouseid
						FOR JSON PATH
					) AS Propertyhousesize,
					(
						SELECT 
							ISNULL(Systempropertyhousedepositfee.Systempropertyhousedepositfeeid, 0) AS Systempropertyhousedepositfeeid,
							ISNULL(Systempropertyhousedepositfee.Propertyhouseid, 0) AS Propertyhouseid,
							ISNULL(Systemhousedepositfee.Housedepositfeeid, 0) AS Housedepositfeeid,
							ISNULL(Systemhousedepositfee.Housedepositfeename, '') AS Housedepositfeename,
							ISNULL(Systempropertyhousedepositfee.Systempropertyhousedepositfeeamount, 0) AS Systempropertyhousedepositfeeamount,
							ISNULL(Systempropertyhousedepositfee.Systempropertyhousesizedepositfeewehave, 0) AS Systempropertyhousesizedepositfeewehave
						FROM Systemhousedepositfees Systemhousedepositfee
						LEFT JOIN Systempropertyhousedepositfees Systempropertyhousedepositfee ON Systemhousedepositfee.Housedepositfeeid = Systempropertyhousedepositfee.Housedepositfeeid
						WHERE Systempropertyhouse.Propertyhouseid=Systempropertyhousedepositfee.Propertyhouseid
						FOR JSON PATH
					) AS Propertyhousedepositfee,
					(
						SELECT 
							ISNULL(Systempropertybankaccount.Systempropertybankaccountid, 0) AS Systempropertybankaccountid,
							ISNULL(Systempropertybankaccount.Propertyhouseid, 0) AS Propertyhouseid,
							ISNULL(Systemsupportedbank.Systembankid, 0) AS Systembankid,
							ISNULL(Systemsupportedbank.Systembankname+' '+ Systemsupportedbank.Systembankpaybill, '') AS Systembanknameandpaybill,
							ISNULL(Systempropertybankaccount.Systempropertybankaccount, 0) AS Systempropertybankaccount,
							ISNULL(Systempropertybankaccount.Systempropertyhousebankwehave, 0) AS Systempropertyhousebankwehave
						FROM Systemsupportedbanks Systemsupportedbank
						LEFT JOIN Systempropertybankaccounts Systempropertybankaccount ON Systemsupportedbank.Systembankid = Systempropertybankaccount.Systembankid
						WHERE Systempropertyhouse.Propertyhouseid=Systempropertybankaccount.Propertyhouseid
						FOR JSON PATH
					) AS Propertyhousebankingdetail,
					(
						SELECT 
							ISNULL(Systempropertyhousebenefit.Systempropertyhousebenefitid, 0) AS Systempropertyhousebenefitid,
							ISNULL(Systempropertyhousebenefit.Propertyhouseid, 0) AS Propertyhouseid,
							ISNULL(Systemhousebenefit.Housebenefitid, 0) AS Housebenefitid,
							ISNULL(Systemhousebenefit.Housebenefitname, '') AS Housebenefitname,
							ISNULL(Systempropertyhousebenefit.Systempropertyhousebenefitwehave, 0) AS Systempropertyhousebenefitwehave
						FROM Systemhousebenefits Systemhousebenefit
						LEFT JOIN Systempropertyhousebenefits Systempropertyhousebenefit ON Systemhousebenefit.Housebenefitid = Systempropertyhousebenefit.Housebenefitid
						WHERE Systempropertyhouse.Propertyhouseid=Systempropertyhousebenefit.Propertyhouseid
						FOR JSON PATH
					) AS Propertyhousebenefit
					FROM Systempropertyhouses Systempropertyhouse
					WHERE Systempropertyhouse.Propertyhouseid=@Propertyhouseid
					FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
				 );
			END
	    Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@Systempropertydata AS Systempropertydata;

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