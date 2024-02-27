using DataLibrary.Entities;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.ResetPassword
{
    public interface IReadResetPasswordRepository
    {
        Task<GetResetPasswordResponse?> GetResetPasswordByIdAsync(int passwordResetId);
        Task<RESET_PASSWORD?> GetLastAdded(int userId);
    }
}
