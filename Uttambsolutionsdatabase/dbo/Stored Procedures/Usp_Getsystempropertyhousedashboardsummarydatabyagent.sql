CREATE PROCEDURE [dbo].[Usp_Getsystempropertyhousedashboardsummarydatabyagent]
@Agentid INT,
@Systempropertyhousedashboardsummarydata NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Main query for dashboard summary data
        SELECT @Systempropertyhousedashboardsummarydata = 
        (
           SELECT(SELECT 
                COUNT(Systempropertyhouseroom.Systempropertyhouseid) AS Propertyhouseunits,
                SUM(CASE WHEN Systempropertyhouseroom.Isvacant = 0 THEN 1 ELSE 0 END) AS Systempropertyoccupiedroom,
                SUM(CASE WHEN Systempropertyhouseroom.Isvacant = 1 THEN 1 ELSE 0 END) AS Systempropertyvacantroom,
                SUM(Systempropertyhouseroom.Systempropertyhousesizerent) AS Expectedcollections,
                SUM(INV.Paidamount) AS Collectedcollections,
                SUM(INV.Balance) AS Rentarrears,
                SUM(INV.Balance) AS Uncollectedpayments,
                ISNULL(SUM(Systempropertyhouseroommeter.Movedmeter), 0) AS Consumedmeters,
                (
                    SELECT 
                        Systempropertyhousedata.Propertyhousename,
                        COUNT(Systempropertyhouseroom.Systempropertyhouseid) AS Propertyhouseunits,
                        SUM(CASE WHEN Systempropertyhouseroom.Isvacant = 0 THEN 1 ELSE 0 END) AS Systempropertyoccupiedroom,
                        SUM(CASE WHEN Systempropertyhouseroom.Isvacant = 1 THEN 1 ELSE 0 END) AS Systempropertyvacantroom,
                        SUM(Systempropertyhouseroom.Systempropertyhousesizerent) AS Expectedcollections,
                        SUM(INV.Paidamount) AS Collectedcollections,
                        SUM(INV.Balance) AS Rentarrears,
                        ISNULL(SUM(Systempropertyhouseroommeter.Movedmeter), 0) AS Consumedmeters
                    FROM Systempropertyhouses Systempropertyhousedata
                    INNER JOIN Systempropertyhouserooms Systempropertyhouseroom 
                        ON Systempropertyhousedata.Propertyhouseid = Systempropertyhouseroom.Systempropertyhouseid
                    LEFT JOIN Monthlyrentinvoices INV 
                        ON Systempropertyhouseroom.Systempropertyhouseroomid = INV.Propertyhouseroomid
                    LEFT JOIN Systempropertyhouseroommeters Systempropertyhouseroommeter 
                        ON Systempropertyhouseroom.Systempropertyhouseroomid = Systempropertyhouseroommeter.Systempropertyhouseroomid
                    WHERE Systempropertyhousedata.Propertyhouseposter = @Agentid
                    GROUP BY Systempropertyhousedata.Propertyhousename
                    FOR JSON PATH
                ) AS Propertybysummary
            FROM Systempropertyhouses Systempropertyhouse
            INNER JOIN Systempropertyhouserooms Systempropertyhouseroom 
                ON Systempropertyhouse.Propertyhouseid = Systempropertyhouseroom.Systempropertyhouseid
            LEFT JOIN Monthlyrentinvoices INV 
                ON Systempropertyhouseroom.Systempropertyhouseroomid = INV.Propertyhouseroomid
            LEFT JOIN Systempropertyhouseroommeters Systempropertyhouseroommeter 
                ON Systempropertyhouseroom.Systempropertyhouseroomid = Systempropertyhouseroommeter.Systempropertyhouseroomid
            WHERE Systempropertyhouse.Propertyhouseposter = @Agentid
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