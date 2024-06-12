using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
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

    }
}
