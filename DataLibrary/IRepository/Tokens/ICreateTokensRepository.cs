using DataLibrary.Model.DTO.Request;

namespace DataLibrary.IRepository.Tokens
{
    public interface ICreateTokensRepository
    {
        Task AddAccessTokenAsync(GetAccessTokenRequest accessTokesInsert);
        Task AddRefreshTokenAsync(GetRefreshTokenRequest refreshTokenInsert);
    }
}
