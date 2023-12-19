using DataLibrary.Model.DTO.Request;

namespace BLLLibrary.IService
{
    public interface IEmailSenderService
    {
        Task<bool> SendInviteMessageAsync(GetEmailSenderRequest getEmailSenderRequest, CancellationToken ct);
    }
}
