using DataLibrary.Model.DTO.Request;

namespace DataLibrary.IRepository.GroupInvite
{
    public interface ICreateGroupInviteRepository
    {
        Task AddGroupInviteAsync(GetGroupInviteRequest groupInvite);
    }
}
