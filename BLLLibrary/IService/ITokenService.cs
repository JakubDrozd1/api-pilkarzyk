using DataLibrary.Model.DTO;
using FirebirdSql.Data.FirebirdClient;

namespace BLLLibrary
{
    public interface ITokenService
    {
        Task<GetTokenResponse> GenerateJwtTokenAsync(GetTokenRequest tokenRequest, FbTransaction? transaction = null);
        Task SaveChangesAsync();
    }
}
