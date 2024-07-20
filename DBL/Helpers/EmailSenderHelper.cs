using System.Net;
using System.Net.Mail;

namespace DBL.Helpers
{
    public class EmailSenderHelper
    {
        public bool UttambsolutionssendemailAsync(string to, string subject, string body, bool IsBodyHtml, string EmailServer, string EmailServerEmail, string EmailServerPassword)
        {
            //string appServer = "mail.uttambsolutions.com";
            //string appEmail = "communications@uttambsolutions.com";
            //string appPassword = "K@ribun1";

            string appServer = "smtp.gmail.com";
            string appEmail = "uttambsolutions3@gmail.com";
            string appPassword = "fhuq etym dxel pmzw";



            if (EmailServer == null || EmailServer == "" || EmailServerEmail == null || EmailServerEmail == "" || EmailServerPassword == null || EmailServerPassword == "")
            {
                appServer = appServer;
                appEmail = appEmail;
                appPassword = appPassword;
            }
            else
            {
                appServer = EmailServer;
                appEmail = EmailServerEmail;
                appPassword = EmailServerPassword;
            }
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(appEmail, "UTTAMB SOLUTIONS");
                mail.IsBodyHtml = IsBodyHtml;
                mail.Subject = subject;
                mail.Body = body;
                mail.To.Add(to);
                using (SmtpClient smtp = new SmtpClient(appServer, 587))
                {
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(appEmail, appPassword);
                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception here
                        return false;
                    }
                }
            }
        }
    }
}