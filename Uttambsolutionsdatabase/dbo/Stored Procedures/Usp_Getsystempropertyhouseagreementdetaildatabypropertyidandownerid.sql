CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhouseagreementdetaildatabypropertyidandownerid]
@Propetyhouseid BIGINT,
@Ownerortenantid BIGINT,
@OwnerTenantAgreementDetailData VARCHAR(MAX)  OUTPUT
AS
BEGIN
   BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'Ok';
			
			BEGIN
	
		BEGIN TRY
		BEGIN TRANSACTION;
		SET @OwnerTenantAgreementDetailData = (SELECT (SELECT HSC.Propertyhouseid,HSC.Propertyhouseowner,HSC.Propertyhousename AS Propertyhousename,OWN.Firstname+' '+OWN.Lastname AS Fullname,OWN.Phonenumber,OWN.Emailaddress,Systemcounty.Countyname,Systemsubcounty.Subcountyname,Systemsubcountyward.Subcountywardname,
			ISNULL(SOA.Datecreated,GETDATE()) AS OwnerDatecreated,ISNULL(SOA.Signatureimageurl,'') AS OwnerSignatureimageurl
			FROM Systempropertyhouses HSC
			INNER JOIN Systemstaffs OWN ON HSC.Propertyhouseowner= OWN.UserId
			INNER JOIN Systemcounty Systemcounty ON HSC.Countyid=Systemcounty.Countyid
			INNER JOIN Systemsubcounty Systemsubcounty ON HSC.Subcountyid=Systemsubcounty.Subcountyid
			INNER JOIN Systemsubcountyward Systemsubcountyward ON HSC.Subcountywardid=Systemsubcountyward.Subcountywardid
			LEFT JOIN Systempropertyhouseagreements SOA ON HSC.Propertyhouseowner=SOA.Ownerortenantid
			WHERE HSC.Propertyhouseowner= @Ownerortenantid AND HSC.Propertyhouseid =@Propetyhouseid
			FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			) AS Data
			FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
			);
		Set @RespMsg ='Ok.'
		Set @RespStat =0; 
		COMMIT TRANSACTION;

		SELECT  @RespStat as RespStatus, @RespMsg as RespMessage,@OwnerTenantAgreementDetailData AS OwnerTenantAgreementDetailData

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