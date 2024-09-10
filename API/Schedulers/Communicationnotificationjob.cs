using DBL;
using DBL.Entities;
using Quartz;
using System.Text;

namespace API.Schedulers
{
    public class Communicationnotificationjob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly BL bl;
        public Communicationnotificationjob(IServiceProvider provider, IConfiguration config)
        {
            _provider = provider;
            bl = new BL(Util.ShareConnectionString(config));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Logs($"{DateTime.Now} [Reminders Service called]" + Environment.NewLine);

            DateTime now = DateTime.Now;
            DateTime yesterday = now.AddDays(-1);
            string formattedDate = yesterday.ToString("yyyy-MM-dd");
            var unsentEmailAddress = await bl.Getsystemunsentemaildata();
            if (unsentEmailAddress != null && unsentEmailAddress.Data.Any())
            {
                foreach (var invoice in unsentEmailAddress.Data)
                {
                    string companyname = "Maqao Plus";
                    string companyemail = "maqaoplus@uttambsolutions.com";
                    string changepasswordurl = "https://uttambsolutions.com/Account/changepassword";
                    //send email for reseting password
                    string logoUrl = "https://uttambsolutions.com/images/uttambsolutionlogo.png";
                    StringBuilder invoiceHtml = new StringBuilder();

                    // Start of HTML structure
                    invoiceHtml.Append("<div style='font-family: Arial, sans-serif; color: #333; line-height: 1.6;'>");
                    invoiceHtml.Append("<table style='width: 100%; max-width: 600px; margin: 0 auto; border-collapse: collapse; position: relative;'>");

                    // Header with logo and email
                    invoiceHtml.Append("<thead style='background-color: #0a506c; color: #fff; position: relative;'>");
                    invoiceHtml.Append("<tr>");
                    invoiceHtml.Append("<th rowspan='2' style='padding: 15px; text-align: left;'>");
                    invoiceHtml.Append($"<img src=\"{logoUrl}\" alt=\"{companyname}\" style='max-width: 120px; max-height: 120px;' />");
                    invoiceHtml.Append("</th>");
                    invoiceHtml.Append($"<th colspan='2' style='padding: 15px; text-align: right; font-size: 18px;'>\"{companyname}\"</th>");
                    invoiceHtml.Append("</tr>");
                    invoiceHtml.Append("<tr>");
                    invoiceHtml.Append("<th colspan='2' style='padding: 10px; text-align: right; font-size: 14px;'>");
                    invoiceHtml.Append($"Email: <a href='mailto:\"{companyemail}\"' style='color: #fff; text-decoration: none;'>\"{companyemail}\"</a>");
                    invoiceHtml.Append("</th>");
                    invoiceHtml.Append("</tr>");

                    // Ribbon at the far right for payment status
                    string paidStatus = invoice.IsPaid ? "Paid" : "Unpaid";
                    invoiceHtml.Append("<div style='position: absolute; top: 0; right: 0; width: 150px; height: 50px; overflow: hidden; z-index: 1;'>");
                    invoiceHtml.Append($"<div style='background-color: {(invoice.IsPaid ? "green" : "red")}; color: white; padding: 8px 0; width: 220px; text-align: center; transform: rotate(45deg) translate(40px, -70px);'>");
                    invoiceHtml.Append($"{paidStatus}");
                    invoiceHtml.Append("</div>");
                    invoiceHtml.Append("</div>");

                    invoiceHtml.Append("</thead>");

                    // Invoice Section with Invoice Number and Date
                    invoiceHtml.Append("<tbody>");
                    invoiceHtml.Append("<tr><td style='border:none; text-align:right;'><h2>INVOICE</h2></td></tr>");
                    invoiceHtml.Append("<tr>");
                    invoiceHtml.Append("<td style='border:none; text-align:left;'>");
                    invoiceHtml.Append($"<strong>From:</strong><br>Uttamb Solutions<br>info@uttambsolutions.com");
                    invoiceHtml.Append("</td>");
                    invoiceHtml.Append("<td style='border:none; text-align:right;'>");
                    invoiceHtml.Append($"<strong>Invoice No:</strong> {invoice.InvoiceNo}<br>");
                    invoiceHtml.Append($"<strong>Date:</strong> {invoice.DateCreated.ToString("dd/MM/yyyy")}");
                    invoiceHtml.Append("</td>");
                    invoiceHtml.Append("</tr>");

                    // Customer Info
                    invoiceHtml.Append("<tr>");
                    invoiceHtml.Append("<td colspan='2' style='border:none; padding-top: 10px;'>");
                    invoiceHtml.Append("<strong>Bill To:</strong><br>");
                    invoiceHtml.Append($"{invoice.FullName}<br>");
                    invoiceHtml.Append($"{invoice.EmailAddress}<br>");
                    invoiceHtml.Append($"{invoice.PhoneNumber}");
                    invoiceHtml.Append("</td>");
                    invoiceHtml.Append("</tr>");

                    // Invoice Details Header
                    invoiceHtml.Append("<tr>");
                    invoiceHtml.Append("<td style='border: 1px solid #ddd; padding: 8px;'><strong>Description</strong></td>");
                    invoiceHtml.Append("<td style='border: 1px solid #ddd; padding: 8px; text-align:right;'><strong>Units</strong></td>");
                    invoiceHtml.Append("<td style='border: 1px solid #ddd; padding: 8px; text-align:right;'><strong>Price</strong></td>");
                    invoiceHtml.Append("</tr>");

                    // Loop through the InvoiceDetails
                    foreach (var detail in invoice.InvoiceDetails)
                    {
                        invoiceHtml.Append("<tr>");
                        invoiceHtml.Append($"<td style='border: 1px solid #ddd; padding: 8px;'>{detail.HouseDepositFeeName}</td>");
                        invoiceHtml.Append($"<td style='border: 1px solid #ddd; padding: 8px; text-align:right;'>{detail.Units}</td>");
                        invoiceHtml.Append($"<td style='border: 1px solid #ddd; padding: 8px; text-align:right;'>Ksh{detail.Price:0.00}</td>");
                        invoiceHtml.Append("</tr>");
                    }

                    // Total Amount
                    invoiceHtml.Append("<tr>");
                    invoiceHtml.Append("<td colspan='2' style='border: 1px solid #ddd; padding: 8px; text-align:right;'><strong>Total</strong></td>");
                    invoiceHtml.Append($"<td style='border: 1px solid #ddd; padding: 8px; text-align:right;'><strong>Ksh{invoice.Amount:0.00}</strong></td>");
                    invoiceHtml.Append("</tr>");

                    // Footer Note based on payment status
                    invoiceHtml.Append("<tr>");
                    invoiceHtml.Append("<td colspan='3' style='border:none; padding: 20px 0 0 0; text-align:left;'>");
                    invoiceHtml.Append("<strong>Note:</strong> This invoice is currently ");
                    invoiceHtml.Append(invoice.IsPaid ? "<span style='color: green;'>paid</span>" : "<span style='color: red;'>unpaid</span>");
                    invoiceHtml.Append(". Please make payment by the due date to avoid late fees.");
                    invoiceHtml.Append("</td>");
                    invoiceHtml.Append("</tr>");

                    invoiceHtml.Append("</tbody>");

                    // Footer Section
                    invoiceHtml.Append("<tfoot style='background-color: #0a506c; color: #fff;'>");
                    invoiceHtml.Append("<tr>");
                    invoiceHtml.Append("<td colspan='3' style='padding: 15px; text-align: center; font-size: 14px;'>");
                    invoiceHtml.Append("Uttamb Solutions &copy; 2022 - 2024");
                    invoiceHtml.Append("</td>");
                    invoiceHtml.Append("</tr>");
                    invoiceHtml.Append("<tr>");
                    invoiceHtml.Append("<td colspan='3' style='padding: 10px; text-align: center; font-size: 12px;'>");
                    invoiceHtml.Append("Vision: Utilizing Technology To Automate Modern Business");
                    invoiceHtml.Append("</td>");
                    invoiceHtml.Append("</tr>");
                    invoiceHtml.Append("<tr>");
                    invoiceHtml.Append("<td colspan='3' style='padding: 10px; text-align: center; font-size: 12px;'>");
                    invoiceHtml.Append("Mission: For Quality and Value");
                    invoiceHtml.Append("</td>");
                    invoiceHtml.Append("</tr>");
                    invoiceHtml.Append("</tfoot>");

                    invoiceHtml.Append("</table>");
                    invoiceHtml.Append("</div>");

                    // string message = bl.GenerateEmailBody(logoUrl, "Uttamb Solutions", "info@uttambsolutions.com", invoiceHtml.ToString(), DateTime.Now.Year.ToString());

                    //log Email Messages
                    EmailLogs Logs = new EmailLogs
                    {
                        EmailLogId = 0,
                        ModuleId = 1,
                        EmailAddress = invoice.EmailAddress,
                        EmailSubject = "Forgot Password",
                        EmailMessage = invoiceHtml.ToString(),
                        IsEmailSent = false,
                        DateTimeSent = DateTime.Now,
                        Datecreated = DateTime.Now,
                    };
                    var respdata = await bl.LogEmailMessage(Logs);
                    bool data = await bl.Uttambsolutionssendemail(invoice.EmailAddress, "Monthly Rent Invoice", invoiceHtml.ToString(), true, "", "", "");
                    //Update Email is sent 
                    EmailLogs Logs1 = new EmailLogs
                    {
                        EmailLogId = Convert.ToInt64(respdata.Data1),
                        ModuleId = 1,
                        EmailAddress = invoice.EmailAddress,
                        EmailSubject = "Forgot Password",
                        EmailMessage = invoiceHtml.ToString(),
                        IsEmailSent = true,
                        DateTimeSent = DateTime.Now,
                        Datecreated = DateTime.Now,
                    };
                    var resp1 = await bl.LogEmailMessage(Logs1);
                    await bl.Updatemonthlyrentinvoicedata(invoice.InvoiceId);
                }
                await Task.CompletedTask;
            }
        }

        public void Logs(string message)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "QuartzInvoices");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, "Logs.txt");
            using FileStream fstream = new FileStream(path, FileMode.Append);
            using TextWriter writer = new StreamWriter(fstream);
            writer.WriteLine(message);
        }
    }
}
