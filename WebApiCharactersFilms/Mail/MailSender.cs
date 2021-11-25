using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiCharactersFilms.Constants;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebApiCharactersFilms.Models;

namespace WebApiCharactersFilms.Mail
{
    public class MailSender : IMailSender
    {
        private readonly MailSettings _mailSettings;
        public MailSender(IOptions<MailSettings> mailSettings) {
            _mailSettings = mailSettings.Value;
        }

        public async Task EmailChangePassword(MailRequest mailRequest) {
            string aux = Constants.Mail.SendNewPassword;
            mailRequest.Body = aux.Replace("{MI_TEXTO}", mailRequest.Body);
            await SendEmailAsync(mailRequest);
        }

        public async Task EmailConfirmRegister(MailRequest mailRequest) {
            string aux = Constants.Mail.ConfirmRegister;
            mailRequest.Body = aux.Replace("{MI_TEXTO}", mailRequest.Body);
            await SendEmailAsync(mailRequest);
        }

        public async Task SendEmailAsync(MailRequest mailRequest) {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();

            message.From = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
            message.To.Add(new MailAddress(mailRequest.ToEmail));
            message.Subject = mailRequest.Subject;

            message.IsBodyHtml = true;
            message.Body = mailRequest.Body;

            smtp.Port = _mailSettings.Port;
            smtp.Host = _mailSettings.Host;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            await smtp.SendMailAsync(message);
        }
    }
}
