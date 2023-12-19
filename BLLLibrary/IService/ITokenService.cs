using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace BLLLibrary.IService
{
    public interface ITokenService
    {
        Task<GetTokenResponse> GenerateJwtTokenAsync(GetTokenRequest tokenRequest, FbTransaction? transaction = null);
        Task SaveChangesAsync();
    }
}
