using System.Text;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.UoW;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Security.Cryptography;
using System;
using BLLLibrary.IService;

namespace BLLLibrary.Service
{
    public class EmailSenderService(IUnitOfWork unitOfWork, IConfiguration configuration) : IEmailSenderService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _configuration = configuration;

        public async Task<bool> SendInviteMessageAsync(GetEmailSenderRequest getEmailSenderRequest, CancellationToken ct = default)
        {
            try
            {
                string? email = _configuration["MailSettings:From"] ?? throw new Exception("Sender not found");
                EMAIL_SENDER? emailSender = await _unitOfWork.ReadEmailSender.GetEmailDetailsAsync(email) ?? throw new Exception("Sender not found");
                var mail = new MimeMessage();
                mail.From.Add(new MailboxAddress(emailSender.DISPLAY_NAME, emailSender.EMAIL));
                mail.Sender = new MailboxAddress(emailSender.DISPLAY_NAME, emailSender.EMAIL);
                mail.To.Add(MailboxAddress.Parse(getEmailSenderRequest.To));
                if (getEmailSenderRequest.BlindCarbonCopy != null)
                {
                    foreach (string mailAddress in getEmailSenderRequest.BlindCarbonCopy.Where(x => !string.IsNullOrWhiteSpace(x)))
                        mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }
                if (getEmailSenderRequest.CarbonCopy != null)
                {
                    foreach (string mailAddress in getEmailSenderRequest.CarbonCopy.Where(x => !string.IsNullOrWhiteSpace(x)))
                        mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }
                var body = new BodyBuilder();
                string encodedGroupId = Convert.ToBase64String(BitConverter.GetBytes(getEmailSenderRequest.IdGroup));
                string link = "http://192.168.88.20:4200/register/" + encodedGroupId;
                string invateSubject = $"Zaproszenie do grupy {getEmailSenderRequest.GroupName}";
                string bodySubject = $"<h1>Hej!</h1>" +
                                    $"<p>{getEmailSenderRequest.Name} {getEmailSenderRequest.Surname} wysłał ci zaproszenie do grupy {getEmailSenderRequest.GroupName}</p>" +
                                    $"<p>Kliknij, aby założyć konto i dołączyć <a href=\"{link}\" style=\"text-decoration: underline; color: black;\" onmouseover=\"this.style.color='blue'\" onmouseout=\"this.style.color='black'\">tutaj</a></p>";
                mail.Subject = invateSubject;
                body.HtmlBody = bodySubject;
                mail.Body = body.ToMessageBody();
                string host = KeyDecrypt(Convert.FromBase64String(emailSender.SALT), Convert.FromBase64String(emailSender.HOST));
                string emailPassword = KeyDecrypt(Convert.FromBase64String(emailSender.SALT), Convert.FromBase64String(emailSender.EMAIL_PASSWORD));
                using var smtp = new SmtpClient();
                if (emailSender.SSL)
                {
                    await smtp.ConnectAsync(host, emailSender.PORT, SecureSocketOptions.SslOnConnect, ct);
                }
                else if (emailSender.TLS)
                {
                    await smtp.ConnectAsync(host, emailSender.PORT, SecureSocketOptions.StartTls, ct);
                }
                else
                {
                    await smtp.ConnectAsync(host, emailSender.PORT, cancellationToken: ct);
                }
                await smtp.AuthenticateAsync(emailSender.EMAIL, emailPassword, ct);
                await smtp.SendAsync(mail, ct);
                await smtp.DisconnectAsync(true, ct);

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string KeyDecrypt(byte[] key, byte[] dane)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            byte[] iv = new byte[aes.IV.Length];
            Array.Copy(dane, 0, iv, 0, aes.IV.Length);
            aes.IV = iv;
            using var decryptor = aes.CreateDecryptor();
            byte[] decryptedData = decryptor.TransformFinalBlock(dane, aes.IV.Length, dane.Length - aes.IV.Length);
            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}