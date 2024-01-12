namespace DataLibrary.IRepository.GroupInvite
{
    public interface IDeleteGroupInviteRepository
    {
        Task DeleteGroupInviteAsync(int groupInviteId);
    }
}
