using DataLibrary.Model.DTO;
using DataLibrary.UoW;
using FirebirdSql.Data.FirebirdClient;

namespace BLLLibrary.Service
{
    public class TokenService(IUnitOfWork unitOfWork) : ITokenService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<GetTokenResponse> GenerateJwtTokenAsync(GetTokenRequest tokenRequest, FbTransaction? transaction = null)
        {
            return await _unitOfWork.CreateTokensRepository.GenerateJwtTokenAsync(tokenRequest, transaction);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
