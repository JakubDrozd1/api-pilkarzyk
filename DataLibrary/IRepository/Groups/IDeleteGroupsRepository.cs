namespace DataLibrary.IRepository.Groups
{
    public interface IDeleteGroupsRepository
    {
        Task DeleteGroupAsync(int groupId);
    }
}
