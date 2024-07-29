using BLLLibrary.IService;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class AdsService(IUnitOfWork unitOfWork) : IAdsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<GetAdResponse?> GetAdsAsync()
        {
            return await _unitOfWork.ReadAdsRepository.GetAdsAsync();
        }

        public async Task<List<GetAdResponse>> GetAllAdsAsync()
        {
            return await _unitOfWork.ReadAdsRepository.GetAllAdsAsync();

        }
        public async Task PostClickHistoryAd(PostAdHistoryRequest AdHistory)
        {
            await _unitOfWork.CreateAdsRepository.AddClickToHistoryAsync(AdHistory);
        }

    }
}
