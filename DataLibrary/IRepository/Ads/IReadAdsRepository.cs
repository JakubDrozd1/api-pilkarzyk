using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.Ads
{
    public interface IReadAdsRepository
    {
        Task<GetAdResponse?> GetAdsAsync();
        Task<List<GetAddWithClicksResponse>> GetAllAdsAsync();

    }
}
