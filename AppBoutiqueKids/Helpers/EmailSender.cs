using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Helpers
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            MailAddress to = new MailAddress(email);
            MailAddress from = new MailAddress("ben@contoso.com");
            MailMessage message = new MailMessage(from, to);
            message.Subject = subject;
            message.Body = @"Using this new feature, you can send an email message from an application very easily.";
            // Use the application or machine configuration to get the 
            // host, port, and credentials.
            SmtpClient client = new SmtpClient();
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("email", "password");// hotmail email i pwd
            client.Send(message);
            return Task.CompletedTask;
        }
    }
}
