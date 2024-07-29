using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.Users
{
    public interface IReadAdsRepository
    {
        Task<GetAdResponse?> GetAdsAsync();
        Task<List<GetAddWithClicksResponse>> GetAllAdsAsync();

    }
}
