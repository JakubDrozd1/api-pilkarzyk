using DataLibrary.Model.DTO.Request;

namespace DataLibrary.EmailSender
{
    public interface IEmailSenderService
    {
        Task<bool> SendInviteMessageAsync(GetEmailSenderRequest getEmailSenderRequest, CancellationToken ct);
    }
}
