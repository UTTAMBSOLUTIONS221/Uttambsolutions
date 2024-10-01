CREATE TABLE [dbo].[Softwaretokenpurchase] (
    [Tokenpurchaseid] INT             IDENTITY (1, 1) NOT NULL,
    [Tokenid]         INT             NOT NULL,
    [Userid]          INT             NOT NULL,
    [Tokenamount]     DECIMAL (10, 2) NOT NULL,
    [Totalcost]       DECIMAL (10, 2) NOT NULL,
    [Tokenstatus]     INT             DEFAULT ((3)) NOT NULL,
    [Purchasedate]    DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([Tokenpurchaseid] ASC)
);


GO
CREATE TRIGGER trg_AfterUpdate_Tokenownership
ON Softwaretokenpurchase
AFTER UPDATE
AS
BEGIN
    MERGE INTO Tokenownership AS target
    USING (SELECT Tokenid,Userid,Tokenamount FROM INSERTED) AS source
    ON target.Tokenid = source.Tokenid AND target.Userid = source.Userid 
    WHEN MATCHED THEN
    UPDATE SET target.Tokenamount = target.Tokenamount + source.Tokenamount
    WHEN NOT MATCHED BY TARGET THEN
    INSERT (Tokenid, Userid, Tokenamount)
    VALUES (source.Tokenid, source.Userid, source.Tokenamount);
END;