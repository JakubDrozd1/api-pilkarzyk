using DataLibrary.Model.DTO.Request;

namespace BLLLibrary.IService
{
    public interface IEmailSenderService
    {
        Task<bool> SendInviteMessageAsync(GetEmailSenderRequest getEmailSenderRequest, CancellationToken ct);
        Task<bool> SendRecoveryPasswordMessageAsync(string email, int resetPasswordId, CancellationToken ct);
    }
}
