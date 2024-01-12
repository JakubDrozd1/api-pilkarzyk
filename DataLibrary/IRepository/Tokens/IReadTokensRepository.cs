using DataLibrary.Entities;

namespace DataLibrary.IRepository.Tokens
{
    public interface IReadTokensRepository
    {
        Task<ACCESS_TOKENS?> GetTokenByUserAsync(int userId);
        Task<REFRESH_TOKENS?> GetRefreshTokenAsync(string clientId);
        Task<CLIENT_TOKENS?> GetClientTokenByUserAsync(string refreshToken);
    }
}
