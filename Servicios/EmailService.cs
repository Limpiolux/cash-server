using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.IO;
using cash_server.Models;

namespace cash_server.Servicios
{
    public class EmailService
    {
        private readonly string smtpHost = "smtp.office365.com";
        private readonly int smtpPort = 587;
        private readonly string smtpUser = "infovisitasupervisores@limpiolux.com.ar"; // pautomate@limpiolux.com.ar infovisitasupervisores@limpiolux.com.ar
        private readonly string smtpPass = "Pendrive.9274"; // Sard1na.3400 Pendrive.9274

        public void SendEmailWithAttachments(string toEmail, string subject, string body, List<EmailAttachment> attachments)
        {
            var fromAddress = new MailAddress(smtpUser, "Visitas Preventores");
            var toAddress = new MailAddress(toEmail);

            var smtp = new SmtpClient
            {
                Host = smtpHost,
                Port = smtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, smtpPass)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                foreach (var attachment in attachments)
                {
                    message.Attachments.Add(new Attachment(new MemoryStream(attachment.Content), attachment.FileName));
                }

                smtp.Send(message);
            }
        }
    }
}