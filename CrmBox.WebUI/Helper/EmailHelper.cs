using System.Net;
using System.Net.Mail;
using System.Security.Policy;
using System.Web;
using CrmBox.Core.Domain.Identity;
using CrmBox.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Web;
namespace CrmBox.WebUI.Helper
{
    public class EmailHelper
    {
        public bool SendEmail(string userEmail, string confirmationLink)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("melih16-meral@hotmail.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm your email";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = confirmationLink;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("melih16-meral@hotmail.com", "Ed4b122ff.");
            client.Host = "smtp-mail.outlook.com";
            client.Port = 587;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }

        public bool SendEmailPasswordReset(string userEmail, string link)
        {
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.To.Add(new MailAddress(userEmail));
            mail.From = new MailAddress("melih16-meral@hotmail.com", "Şifre Güncelleme", System.Text.Encoding.UTF8);
            mail.Subject = "Şifre Güncelleme Talebi - CrmBox";
            mail.Body = link;
            mail.IsBodyHtml = true;

            SmtpClient smp = new SmtpClient();
            smp.UseDefaultCredentials = false;
            smp.Credentials = new NetworkCredential("melih16-meral@hotmail.com", "Ed4b122ff.");
            smp.Port = 587;
            smp.Host = "smtp-mail.outlook.com";
            smp.EnableSsl = true;
            smp.Send(mail);

            try
            {
                smp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }

        public bool SendEmailTwoFactorCode(string userEmail, string code) 
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("melih16.meral@hotmail.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Two Factor Code";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = code;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("melhi16-meral@hotmail.com", "Ed4b122ff.");
            client.Host = "smtp-mail.outlook.com";
            client.Port = 587;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }
    }
}
