using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.ADS
{
    public interface ICreateAdsRepository
    {
        Task AddClickToHistoryAsync(PostAdHistoryRequest postAdHistoryRequest);
    }
}
