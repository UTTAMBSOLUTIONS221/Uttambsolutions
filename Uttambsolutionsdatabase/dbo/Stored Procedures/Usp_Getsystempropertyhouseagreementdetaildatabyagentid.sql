CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhouseagreementdetaildatabyagentid]
@Agentid BIGINT,
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
		SET @OwnerTenantAgreementDetailData = (SELECT (SELECT OWN.Userid AS Propertyhouseid,OWN.Userid AS Propertyhouseowner,'' AS Propertyhousename,OWN.Firstname+' '+OWN.Lastname AS Fullname,OWN.Phonenumber,OWN.Emailaddress,'' AS Countyname,'' AS Subcountyname,'' AS Subcountywardname,
			ISNULL(SOA.Datecreated,GETDATE()) AS OwnerDatecreated,ISNULL(SOA.Signatureimageurl,'') AS OwnerSignatureimageurl
			FROM Systemstaffs OWN
			LEFT JOIN Systempropertyhouseagreements SOA ON OWN.Userid=SOA.Ownerortenantid
			WHERE OWN.Userid= @Agentid
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