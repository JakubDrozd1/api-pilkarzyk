using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.GroupInvite
{
    public interface IReadGroupInviteRepository
    {
        Task<List<GetGroupInviteResponse?>> GetGroupInviteByIdUserAsync(int userId);
    }
}
