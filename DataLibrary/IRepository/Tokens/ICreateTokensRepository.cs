using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Tokens
{
    public interface ICreateTokensRepository
    {
        Task<GetTokenResponse> GenerateJwtTokenAsync(GetTokenRequest tokenRequest, FbTransaction? transaction = null);
    }
}
