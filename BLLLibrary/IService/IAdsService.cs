using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IAdsService
    {
        Task<GetAdResponse?> GetAdsAsync();
        Task<List<GetAdResponse>> GetAllAdsAsync();
        Task PostClickHistoryAd(PostAdHistoryRequest AdHistory);

    }
}
