CREATE PROCEDURE [dbo].[Usp_Registervalidatecustomerpaymentrequestdata]
    @JsonObjectdata NVARCHAR(MAX)
AS
BEGIN
    DECLARE @RespStat INT = 0,
            @RespMsg VARCHAR(150) = '';

    BEGIN TRY
        BEGIN TRANSACTION;

		
	    DECLARE @RemainingAmount DECIMAL = JSON_VALUE(@JsonObjectdata, '$.Actualamount');
		DECLARE @CurrentInvoiceID INT,@CurrentInvoiceAmount DECIMAL;
		DECLARE InvoiceCursor CURSOR FOR
		SELECT CI.InvoiceId, CI.Balance AS RemainingAmountToPay FROM Monthlyrentinvoices CI WHERE CI.Propertyhouseroomid = JSON_VALUE(@JsonObjectdata, '$.Houseroomid') AND CI.Propertyhouseroomtenantid = JSON_VALUE(@JsonObjectdata, '$.Tenantid') AND CI.Ispaid = 0 ORDER BY CI.Duedate;

		OPEN InvoiceCursor;

		FETCH NEXT FROM InvoiceCursor INTO @CurrentInvoiceID, @CurrentInvoiceAmount;

		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF @RemainingAmount >= @CurrentInvoiceAmount
			BEGIN
			 UPDATE Monthlyrentinvoices SET IsPaid =1,Paidstatus='PAID', PaidAmount=@CurrentInvoiceAmount,Balance=0 WHERE InvoiceId =@CurrentInvoiceID
		      
			  INSERT INTO Invoicesettlement(PaymentId,InvoiceId,Amountused,Datesettled)
			  SELECT JSON_VALUE(@JsonObjectdata, '$.CustomerPaymentId'), @CurrentInvoiceID,@CurrentInvoiceAmount,GETDATE()
				-- Update the remaining amount
				SET @RemainingAmount = @RemainingAmount - @CurrentInvoiceAmount;
			END
			ELSE
			BEGIN
			  UPDATE Monthlyrentinvoices SET IsPaid =0,Paidstatus='Partially Paid',PaidAmount=PaidAmount+@RemainingAmount,Balance=Balance-@RemainingAmount WHERE InvoiceId=@CurrentInvoiceID

			  INSERT INTO Invoicesettlement(PaymentId,InvoiceId,Amountused,Datesettled)
			  SELECT JSON_VALUE(@JsonObjectdata, '$.CustomerPaymentId'),@CurrentInvoiceID,@CurrentInvoiceAmount,GETDATE()

				-- Exit the loop as payment is exhausted
				BREAK;
			END;

			FETCH NEXT FROM InvoiceCursor INTO @CurrentInvoiceID, @CurrentInvoiceAmount;
		END;

		CLOSE InvoiceCursor;
		DEALLOCATE InvoiceCursor;



		UPDATE CustomerPayments SET IsPaymentValidated =1,Actualamount= JSON_VALUE(@JsonObjectdata, '$.Actualamount') WHERE CustomerPaymentId=JSON_VALUE(@JsonObjectdata, '$.CustomerPaymentId')
		UPDATE GLTransactions SET Amount=-1 * CONVERT(decimal(10,4),(JSON_VALUE(@JsonObjectdata, '$.Actualamount'))) WHERE FinanceTransactionId=JSON_VALUE(@JsonObjectdata, '$.Financetransactionid') 
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