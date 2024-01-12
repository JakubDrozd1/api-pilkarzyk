using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.GroupInvite
{
    public interface IReadGroupInviteRepository
    {
        Task<List<GetGroupInviteResponse?>> GetGroupInviteByIdUserAsync(int userId);
    }
}
