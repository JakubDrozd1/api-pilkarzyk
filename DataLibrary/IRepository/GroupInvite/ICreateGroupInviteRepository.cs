using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.GroupInvite
{
    public interface ICreateGroupInviteRepository
    {
        Task AddGroupInviteAsync(GetGroupInviteRequest groupInvite);
    }
}
