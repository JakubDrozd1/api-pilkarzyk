using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.ResetPassword
{
    public interface ICreateResetPasswordRepository
    {
        Task AddResetPasswordAsync(GetResetPasswordRequest getResetPasswordRequest);
    }
}
