using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IResetPasswordService
    {
        Task<RESET_PASSWORD?> AddResetPasswordAsync(string email);
        Task<GetResetPasswordResponse?> GetResetPasswordByIdAsync(int passwordResetId);
        Task<RESET_PASSWORD?> GetLastAdded(int userId);
    }
}
