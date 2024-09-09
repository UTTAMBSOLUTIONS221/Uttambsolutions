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
                foreach (var unsentEmailData in unsentEmailAddress.Data)
                {
                    string companyname = "Maqao Plus";
                    string changepasswordurl = "https://uttambsolutions.com/Account/changepassword";
                    //send email for reseting password
                    StringBuilder StrBodyEmail = new StringBuilder();
                    StringBuilder invoiceHtml = new StringBuilder();

                    // Start invoice body
                    invoiceHtml.Append("<table style=\"width: 100%; border-collapse: collapse; font-family: Arial, sans-serif;\">");

                    // Add the unpaid ribbon
                    invoiceHtml.Append(@"
                                <div style='position:relative;'>
                                    <div style='position:absolute; top:0; right:0; background-color:red; color:white; padding:5px 10px; font-weight:bold;'>
                                        Unpaid
                                    </div>
                                </div>
                            ");

                    // Invoice header
                    invoiceHtml.Append($@"
                            <thead>
                                <tr><td colspan='2' style='border:none;'><img src='https://uttambsolutions.com/images/uttambsolutionlogo.png' alt='Company Logo' style='max-width: 150px;'></td></tr>
                                <tr><td colspan='2' style='border:none; text-align:right;'><h2>Invoice</h2></td></tr>
                                <tr>
                                    <td style='border:none; text-align:left;'><strong>From:</strong><br>{companyname}<br>info@uttambsolutions.com</td>
                                    <td style='border:none; text-align:right;'><strong>Invoice No:</strong> {unsentEmailData.InvoiceNo}<br><strong>Date:</strong> {DateTime.Now.ToShortDateString()}</td>
                                </tr>
                            </thead>
                        ");

                    // Customer details
                    invoiceHtml.Append($@"
                        <tbody>
                            <tr>
                                <td colspan='2' style='border:none; padding-top: 10px;'>
                                    <strong>Bill To:</strong><br>
                                    {unsentEmailData.FullName}<br>
                                    {unsentEmailData.EmailAddress}<br>
                                    {unsentEmailData.PhoneNumber}
                                </td>
                            </tr>
                    ");

                    // Add the items table header
                    invoiceHtml.Append(@"
                                <tr>
                                    <td style='border: 1px solid #ddd; padding: 8px;'><strong>Description</strong></td>
                                    <td style='border: 1px solid #ddd; padding: 8px; text-align:right;'><strong>Units</strong></td>
                                    <td style='border: 1px solid #ddd; padding: 8px; text-align:right;'><strong>Price</strong></td>
                                </tr>
                            ");

                    // Add item rows
                    foreach (var item in unsentEmailData.InvoiceDetails)
                    {
                        invoiceHtml.Append($@"
                            <tr>
                                <td style='border: 1px solid #ddd; padding: 8px;'>{item.HouseDepositFeeName}</td>
                                <td style='border: 1px solid #ddd; padding: 8px; text-align:right;'>{item.Units:C}</td>
                                <td style='border: 1px solid #ddd; padding: 8px; text-align:right;'>{item.Price:C}</td>
                            </tr>
                        ");
                    }

                    // Summary row for total
                    invoiceHtml.Append($@"
                            <tr>
                                <td style='border: 1px solid #ddd; padding: 8px; text-align:right;'><strong>Total</strong></td>
                                <td style='border: 1px solid #ddd; padding: 8px; text-align:right;'><strong>{unsentEmailData.Amount:C}</strong></td>
                            </tr>
                        ");

                    // Add footer note
                    invoiceHtml.Append(@"
                        <tr>
                            <td colspan='2' style='border:none; padding: 20px 0 0 0; text-align:left;'>
                                <strong>Note:</strong> This invoice is currently <span style='color: red;'>unpaid</span>. Please make payment by the due date to avoid late fees.
                            </td>
                        </tr>
                    ");

                    invoiceHtml.Append("</tbody>");
                    invoiceHtml.Append("</table>");


                    string logoUrl = "https://uttambsolutions.com/images/uttambsolutionlogo.png";
                    string message = bl.GenerateEmailBody(logoUrl, "Uttamb Solutions", "info@uttambsolutions.com", invoiceHtml.ToString(), DateTime.Now.Year.ToString());

                    //log Email Messages
                    EmailLogs Logs = new EmailLogs
                    {
                        EmailLogId = 0,
                        ModuleId = 1,
                        EmailAddress = unsentEmailData.EmailAddress,
                        EmailSubject = "Forgot Password",
                        EmailMessage = message,
                        IsEmailSent = false,
                        DateTimeSent = DateTime.Now,
                        Datecreated = DateTime.Now,
                    };
                    var respdata = await bl.LogEmailMessage(Logs);
                    bool data = await bl.Uttambsolutionssendemail(unsentEmailData.EmailAddress, "Monthly Rent Invoice", message, true, "", "", "");
                    //Update Email is sent 
                    EmailLogs Logs1 = new EmailLogs
                    {
                        EmailLogId = Convert.ToInt64(respdata.Data1),
                        ModuleId = 1,
                        EmailAddress = unsentEmailData.EmailAddress,
                        EmailSubject = "Forgot Password",
                        EmailMessage = message,
                        IsEmailSent = true,
                        DateTimeSent = DateTime.Now,
                        Datecreated = DateTime.Now,
                    };
                    var resp1 = bl.LogEmailMessage(Logs1);
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
