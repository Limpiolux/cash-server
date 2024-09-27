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
        private readonly string smtpUser = "visitapreventores@limpiolux.com.ar"; // pautomate@limpiolux.com.ar infovisitasupervisores@limpiolux.com.ar   //visitapreventores@limpiolux.com.ar 
        private readonly string smtpPass = "Microsd.4921"; // Sard1na.3400 --> power    Pendrive.9274 --> infovisitas

        public void SendEmailWithAttachments(string toEmails, string subject, string body, List<EmailAttachment> attachments)
        {
            var fromAddress = new MailAddress(smtpUser, "Visitas Preventores");

            var smtp = new SmtpClient
            {
                Host = smtpHost,
                Port = smtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, smtpPass)
            };

            using (var message = new MailMessage()
            {
                From = fromAddress,
                Subject = subject,
                Body = body
            })
            {
                // Separar las direcciones de correo electrónico por comas y añadirlas como destinatarios
                foreach (var toEmail in toEmails.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.To.Add(new MailAddress(toEmail.Trim()));
                }

                // Adjuntar archivos
                foreach (var attachment in attachments)
                {
                    message.Attachments.Add(new Attachment(new MemoryStream(attachment.Content), attachment.FileName));
                }

                smtp.Send(message);
            }
        }
    }
}