using DataLibrary.Entities;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IResetPasswordService
    {
        Task AddResetPasswordAsync(string email);
        Task<GetResetPasswordResponse?> GetResetPasswordByIdAsync(string passwordResetId);
        Task<RESET_PASSWORD?> GetLastAdded(int userId);
    }
}
