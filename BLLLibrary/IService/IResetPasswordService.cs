using DataLibrary.Entities;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IResetPasswordService
    {
        Task AddResetPasswordAsync(string email);
        Task<GetResetPasswordResponse?> GetResetPasswordByIdAsync(int passwordResetId);
        Task<RESET_PASSWORD?> GetLastAdded(int userId);
    }
}
