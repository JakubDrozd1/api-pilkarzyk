using System.Text;
using DataLibrary.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Security.Cryptography;
using DataLibrary.Model.DTO.Request.EmailRequest;
using Microsoft.Extensions.Configuration;

namespace DataLibrary.Helper.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly MimeMessage? mail = null;
        private readonly SmtpClient? smtp = null;
        private readonly CancellationToken ct;
        private readonly string host;
        private readonly string emailPassword;
        readonly EMAIL_SENDER _emailSender;
        public EmailSender(EMAIL_SENDER emailSender, IConfiguration configuration, CancellationToken ct = default)
        {
            _configuration = configuration;
            _emailSender = emailSender;
            mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(_emailSender.DISPLAY_NAME, _emailSender.EMAIL));
            mail.Sender = new MailboxAddress(_emailSender.DISPLAY_NAME, _emailSender.EMAIL);
            host = KeyDecrypt(Convert.FromBase64String(_emailSender.SALT), Convert.FromBase64String(_emailSender.HOST));
            emailPassword = KeyDecrypt(Convert.FromBase64String(_emailSender.SALT), Convert.FromBase64String(_emailSender.EMAIL_PASSWORD));
            smtp = new SmtpClient();
            this.ct = ct;
        }

        public async Task<bool> SendInviteMessageAsync(GetEmailInvitationGroupRequest getEmailInvitationGroupRequest)
        {
            try
            {
                if (mail != null && smtp != null)
                {
                    mail.To.Add(MailboxAddress.Parse(getEmailInvitationGroupRequest.To));
                    var body = new BodyBuilder();
                    string encodedGroupId = Convert.ToBase64String(BitConverter.GetBytes(getEmailInvitationGroupRequest.IdGroupInvite));
                    string link = _configuration["Angular"] + "/register/" + encodedGroupId;
                    string invateSubject = $"Zaproszenie do grupy {getEmailInvitationGroupRequest.GroupName}";
                    string bodySubject = $"<h1>Hej!</h1>" +
                                        $"<p>{getEmailInvitationGroupRequest.Name} {getEmailInvitationGroupRequest.Surname} wysłał ci zaproszenie do grupy {getEmailInvitationGroupRequest.GroupName}</p>" +
                                        $"<p>Zaproszenie będzie ważne 24 godziny.</p>" +
                                        $"<p>Kliknij, aby założyć konto i dołączyć <a href=\"{link}\" style=\"text-decoration: underline; color: black;\" onmouseover=\"this.style.color='blue'\" onmouseout=\"this.style.color='black'\">tutaj</a></p>";
                    mail.Subject = invateSubject;
                    body.HtmlBody = bodySubject;
                    mail.Body = body.ToMessageBody();
                    return await SendMessageAsync();
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendRecoveryPasswordMessageAsync(GetEmailResetPasswordRequest getEmailResetPassword)
        {
            try
            {
                if (mail != null)
                {
                    mail.To.Add(MailboxAddress.Parse(getEmailResetPassword.To));
                    var body = new BodyBuilder();
                    string encodedResetPasswordId = Convert.ToBase64String(BitConverter.GetBytes(getEmailResetPassword.IdResetPassword));
                    string link = _configuration["Angular"] + "/recovery/" + encodedResetPasswordId;
                    string invateSubject = $"Przypomnienie hasła";
                    string bodySubject = $"<h1>Hej!</h1>" +
                                        $"<p>Kliknij poniższy link, aby zmienić hasło do swojego konta.</p>" +
                                        $"<p>Link będzie ważny 10 minut.</p>" +
                                        $"<p><a href=\"{link}\" style=\"text-decoration: underline; color: black;\" onmouseover=\"this.style.color='blue'\" onmouseout=\"this.style.color='black'\">RESET</a></p>";
                    mail.Subject = invateSubject;
                    body.HtmlBody = bodySubject;
                    mail.Body = body.ToMessageBody();
                    return await SendMessageAsync();
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> SendMessageAsync()
        {
            if (smtp != null)
            {
                if (_emailSender.SSL)
                {
                    await smtp.ConnectAsync(host, _emailSender.PORT, SecureSocketOptions.SslOnConnect, ct);
                }
                else if (_emailSender.TLS)
                {
                    await smtp.ConnectAsync(host, _emailSender.PORT, SecureSocketOptions.StartTls, ct);
                }
                else
                {
                    await smtp.ConnectAsync(host, _emailSender.PORT, cancellationToken: ct);
                }
                await smtp.AuthenticateAsync(_emailSender.EMAIL, emailPassword, ct);
                await smtp.SendAsync(mail, ct);
                await smtp.DisconnectAsync(true, ct);
                return true;
            }
            return false;
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