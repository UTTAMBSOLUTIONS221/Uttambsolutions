CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhousedashboardsummarydatabyowner]
@Ownerid INT,
@Systempropertyhousedashboardsummarydata NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Main query for dashboard summary data
       SELECT @Systempropertyhousedashboardsummarydata = 
        (
           SELECT(
			   SELECT ISNULL((SELECT COUNT(RMS.Systempropertyhouseroomid) FROM Systempropertyhouserooms RMS INNER JOIN Systempropertyhouses HSC ON RMS.Systempropertyhouseid=HSC.Propertyhouseid WHERE HSC.Propertyhouseposter =@Ownerid),0) AS Propertyhouseunits,
				ISNULL((SELECT COUNT(RMS.Systempropertyhouseroomid) FROM Systempropertyhouserooms RMS INNER JOIN Systempropertyhouses HSC ON RMS.Systempropertyhouseid=HSC.Propertyhouseid WHERE RMS.Isvacant=0 AND HSC.Propertyhouseposter =@Ownerid),0) AS Systempropertyoccupiedroom,
				ISNULL((SELECT COUNT(RMS.Systempropertyhouseroomid) FROM Systempropertyhouserooms RMS INNER JOIN Systempropertyhouses HSC ON RMS.Systempropertyhouseid=HSC.Propertyhouseid WHERE RMS.Isvacant!=0 AND HSC.Propertyhouseposter =@Ownerid),0) AS Systempropertyvacantroom,
				ISNULL((SELECT SUM(RMS.Systempropertyhousesizerent) FROM Systempropertyhouserooms RMS INNER JOIN Systempropertyhouses HSC ON RMS.Systempropertyhouseid=HSC.Propertyhouseid WHERE HSC.Propertyhouseposter =@Ownerid),0) AS Expectedcollections,
				ISNULL((SELECT SUM(INV.Balance) FROM Monthlyrentinvoices INV INNER JOIN Systempropertyhouserooms RMS ON RMS.Systempropertyhouseroomid=INV.Propertyhouseroomid INNER JOIN Systempropertyhouses HSC ON RMS.Systempropertyhouseid=HSC.Propertyhouseid WHERE INV.Invoicestatus='Subsequentinvoice' AND INV.Paidstatus='PARTIALLY PAID' AND HSC.Propertyhouseposter =@Ownerid),0) AS Rentarrears,
				ISNULL((SELECT SUM(INV.Paidamount) FROM Monthlyrentinvoices INV INNER JOIN Systempropertyhouserooms RMS ON RMS.Systempropertyhouseroomid=INV.Propertyhouseroomid INNER JOIN Systempropertyhouses HSC ON RMS.Systempropertyhouseid=HSC.Propertyhouseid WHERE INV.Invoicestatus='Subsequentinvoice' AND HSC.Propertyhouseposter =@Ownerid),0) AS Collectedpayments,
				ISNULL((SELECT SUM(INV.Balance) FROM Monthlyrentinvoices INV INNER JOIN Systempropertyhouserooms RMS ON RMS.Systempropertyhouseroomid=INV.Propertyhouseroomid INNER JOIN Systempropertyhouses HSC ON RMS.Systempropertyhouseid=HSC.Propertyhouseid WHERE INV.Invoicestatus='Subsequentinvoice' AND INV.Paidstatus='NOT PAID' AND HSC.Propertyhouseposter =@Ownerid),0) AS Uncollectedpayments,
				ISNULL((SELECT SUM(MTRS.Movedmeter) FROM Systempropertyhouseroommeters MTRS INNER JOIN Systempropertyhouserooms RMS ON RMS.Systempropertyhouseroomid=MTRS.Systempropertyhouseroomid INNER JOIN Systempropertyhouses HSC ON RMS.Systempropertyhouseid=HSC.Propertyhouseid WHERE HSC.Propertyhouseposter =@Ownerid),0) AS Consumedmeters,
				(SELECT HSC.Propertyhouseid, HSC.Propertyhousename,
					   ISNULL(COUNT(RMS.Systempropertyhouseroomid), 0) AS Propertyhouseunits,
					   ISNULL(SUM(CASE WHEN RMS.Isvacant = 0 THEN 1 ELSE 0 END), 0) AS Systempropertyoccupiedroom,
					   ISNULL(SUM(CASE WHEN RMS.Isvacant != 0 THEN 1 ELSE 0 END), 0) AS Systempropertyvacantroom,
					   ISNULL(SUM(RMS.Systempropertyhousesizerent), 0) AS Expectedcollections,
					   ISNULL((SELECT SUM(INV.Balance) FROM Monthlyrentinvoices INV INNER JOIN Systempropertyhouserooms RMS ON RMS.Systempropertyhouseroomid=INV.Propertyhouseroomid INNER JOIN Systempropertyhouses HSCO ON RMS.Systempropertyhouseid=HSCO.Propertyhouseid WHERE INV.Invoicestatus='Subsequentinvoice' AND INV.Paidstatus='PARTIALLY PAID' AND HSCO.Propertyhouseid =HSC.Propertyhouseid),0) AS Rentarrears,
					   ISNULL((SELECT SUM(INV.Paidamount) FROM Monthlyrentinvoices INV INNER JOIN Systempropertyhouserooms RMS ON RMS.Systempropertyhouseroomid=INV.Propertyhouseroomid INNER JOIN Systempropertyhouses HSCO ON RMS.Systempropertyhouseid=HSCO.Propertyhouseid WHERE INV.Invoicestatus='Subsequentinvoice' AND HSCO.Propertyhouseid =HSC.Propertyhouseid),0) AS Collectedpayments,
					   ISNULL((SELECT SUM(INV.Balance) FROM Monthlyrentinvoices INV INNER JOIN Systempropertyhouserooms RMS ON RMS.Systempropertyhouseroomid=INV.Propertyhouseroomid INNER JOIN Systempropertyhouses HSCO ON RMS.Systempropertyhouseid=HSCO.Propertyhouseid WHERE INV.Invoicestatus='Subsequentinvoice' AND INV.Paidstatus='NOT PAID' AND HSCO.Propertyhouseid =HSC.Propertyhouseid),0) AS Uncollectedpayments,
					   ISNULL((SELECT SUM(MTRS.Movedmeter) FROM Systempropertyhouseroommeters MTRS INNER JOIN Systempropertyhouserooms RMS ON RMS.Systempropertyhouseroomid=MTRS.Systempropertyhouseroomid INNER JOIN Systempropertyhouses HSCO ON RMS.Systempropertyhouseid=HSCO.Propertyhouseid WHERE HSCO.Propertyhouseid =HSC.Propertyhouseid),0) AS Consumedmeters
				FROM Systempropertyhouserooms RMS
				INNER JOIN Systempropertyhouses HSC ON RMS.Systempropertyhouseid = HSC.Propertyhouseid
				WHERE HSC.Propertyhouseposter = @Ownerid
				GROUP BY HSC.Propertyhouseid,HSC.Propertyhousename
				FOR JSON PATH
				) AS Propertybysummary
               FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
			) AS Data
			FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        );

        COMMIT TRANSACTION;

        -- Return the response
        SELECT 0 AS RespStatus, 'Ok' AS RespMessage, @Systempropertyhousedashboardsummarydata AS Systempropertyhousedashboardsummarydata;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 2 AS RespStatus, 'Error(s) Occurred: ' + ERROR_MESSAGE() AS RespMessage;
    END CATCH
END