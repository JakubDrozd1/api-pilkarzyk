using DataLibrary.Model.DTO.Request;

namespace DataLibrary.IRepository.ResetPassword
{
    public interface ICreateResetPasswordRepository
    {
        Task AddResetPasswordAsync(GetResetPasswordRequest getResetPasswordRequest);
    }
}
