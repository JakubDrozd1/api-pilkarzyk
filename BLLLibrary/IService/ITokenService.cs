using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface ITokenService
    {
        Task<GetTokenResponse> GenerateJwtTokenAsync(GetTokenRequest tokenRequest);
        Task SaveChangesAsync();
    }
}
