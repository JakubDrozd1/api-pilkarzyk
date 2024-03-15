using DataLibrary.Model.DTO.Request.EmailRequest;

namespace DataLibrary.Helper.Email
{
    public interface IEmailSender
    {
        Task<bool> SendInviteMessageAsync(GetEmailInvitationGroupRequest getEmailSenderRequest);
        Task<bool> SendRecoveryPasswordMessageAsync(GetEmailResetPasswordRequest getEmailResetPassword);
    }
}
