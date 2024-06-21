using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.Users
{
    public interface IReadAdsRepository
    {
        Task<GetAdResponse?> GetAdsAsync();
    }
}
