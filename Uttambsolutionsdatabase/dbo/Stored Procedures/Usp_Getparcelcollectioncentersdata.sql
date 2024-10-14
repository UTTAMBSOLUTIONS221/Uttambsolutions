CREATE PROCEDURE [dbo].[Usp_Getparcelcollectioncentersdata]
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		  SELECT PCC.Collectioncenterid,PCC.Collectionname,PCC.Phonenumber,PCC.Countyid,SC.Countyname,PCC.Subcountyid,SCC.Subcountyname,PCC.Subcountywardid,SCW.Subcountywardname,
		  PCC.Streetorlandmark,PCC.Maplatitude,PCC.Maplongitude,PCC.Operatinghours,PCC.Collectionstatus,PCC.Managerid,SST.Firstname+' '+SST.Lastname AS Managername
		  FROM Parcelcollectioncenters PCC
		  INNER JOIN Systemcounty SC ON PCC.Countyid=SC.Countyid
		  INNER JOIN Systemsubcounty SCC ON PCC.Subcountyid=SCC.Subcountyid
		  INNER JOIN Systemsubcountyward SCW ON PCC.Subcountywardid=SCW.Subcountywardid
		  INNER JOIN Systemstaffs SST ON PCC.Managerid=SST.Userid
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