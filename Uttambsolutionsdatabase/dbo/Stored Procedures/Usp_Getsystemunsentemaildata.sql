CREATE PROCEDURE [dbo].[Usp_Getsystemunsentemaildata]
    @RentinvoiceDetails NVARCHAR(MAX) OUTPUT
AS
BEGIN
	DECLARE 
			@RespStat int = 0,
			@RespMsg varchar(150) = 'login success';
			BEGIN
				--validate	
		
			BEGIN 
					SET @RentinvoiceDetails = (
						SELECT(SELECT MRI.Invoiceid,MRI.Invoiceno,CASE WHEN HSC.Propertyhouseowner !=HSC.Propertyhouseposter THEN AGNT.Firstname+' '+  AGNT.Lastname ELSE OWN.Firstname+' '+  OWN.Lastname END AS Propertyownername,CASE WHEN HSC.Propertyhouseowner !=HSC.Propertyhouseposter THEN AGNT.Phonenumber ELSE OWN.Phonenumber END AS Ownerphonenumber,CASE WHEN HSC.Propertyhouseowner !=HSC.Propertyhouseposter THEN AGNT.Emailaddress ELSE OWN.Emailaddress END AS Propertyowneremail,STF.Firstname+' '+  STF.Lastname AS Fullname,STF.Emailaddress,STF.Phonenumber,HSC.Propertyhousename,RMM.Systempropertyhousesizename,MRI.Propertyhouseroomid,MRI.Propertyhouseroomtenantid,
							MRI.Financetransactionid,MRI.Datecreated,MRI.Duedate,MRI.Amount,MRI.Discount,MRI.Ispaid,MRI.Paidamount,MRI.Balance,MRI.Issent,MRI.Paidstatus,
							(SELECT  MRID.Invoiceitemid,MRID.Invoiceid,MRID.Systempropertyhousedepositfeeid,SDF.Housedepositfeename,MRID.Units,MRID.Price,MRID.Discount
							FROM Monthlyrentinvoiceitems MRID
							INNER JOIN Systempropertyhousedepositfees SPDF ON MRID.Systempropertyhousedepositfeeid=SPDF.Systempropertyhousedepositfeeid
							INNER JOIN Systemhousedepositfees SDF ON SPDF.Systempropertyhousedepositfeeid=SDF.Housedepositfeeid
							WHERE MRI.Invoiceid= MRID.Invoiceid
							FOR JSON PATH
							) AS InvoiceDetails
							FROM Monthlyrentinvoices MRI
							INNER JOIN Systemstaffs STF ON MRI.Propertyhouseroomtenantid=STF.Userid
							INNER JOIN Systempropertyhouserooms RMM ON MRI.Propertyhouseroomid=RMM.Systempropertyhouseroomid
							INNER JOIN Systempropertyhouses HSC ON RMM.Systempropertyhouseid=HSC.Propertyhouseid
							INNER JOIN Systemstaffs OWN ON HSC.Propertyhouseowner=OWN.Userid
							INNER JOIN Systemstaffs AGNT ON HSC.Propertyhouseposter=AGNT.Userid
							WHERE MRI.Issent=0
							FOR JSON PATH
							)AS Data
							FOR JSON PATH, INCLUDE_NULL_VALUES,WITHOUT_ARRAY_WRAPPER
					);
			END
		END
END