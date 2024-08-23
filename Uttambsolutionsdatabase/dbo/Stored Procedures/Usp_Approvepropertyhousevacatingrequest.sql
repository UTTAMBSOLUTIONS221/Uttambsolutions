CREATE PROCEDURE [dbo].[Usp_Approvepropertyhousevacatingrequest]
    @JsonObjectdata NVARCHAR(MAX)
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '';

    BEGIN TRY
        BEGIN TRANSACTION;
		UPDATE Systempropertyhousevacatingrequests SET Vacatingstatus =0,Approvedby=JSON_VALUE(@JsonObjectdata, '$.Approvedby'),Dateapproved=CAST(JSON_VALUE(@JsonObjectData, '$.Dateapproved') AS DATETIME2) WHERE  Vacatingrequestid= JSON_VALUE(@JsonObjectdata, '$.Vacatingrequestid');

		UPDATE Systempropertyhouseroomstenant SET  Occupationalstatus = 0,Isoccupant= 0,Ratingvalue = JSON_VALUE(@JsonObjectdata, '$.Ratingvalue'),Ownercomment= JSON_VALUE(@JsonObjectdata, '$.Propertyhouseownercomment') WHERE Systempropertyhousetenantid= JSON_VALUE(@JsonObjectdata, '$.Systempropertyhousetenantid') AND Systempropertyhouseroomid= JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroomid');

		UPDATE Systempropertyhouserooms SET  Isvacant = 1 WHERE  Systempropertyhouseroomid= JSON_VALUE(@JsonObjectdata, '$.Systempropertyhouseroomid');

        SET @RespMsg = 'Success.'
        SET @RespStat = 0; 
        
        COMMIT TRANSACTION;

        SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT ''
        PRINT 'Error ' + error_message();
        SELECT 2 AS RespStatus, '0 - Error(s) Occurred' + error_message() AS RespMessage;
    END CATCH

    SELECT @RespStat AS RespStatus, @RespMsg AS RespMessage;
END