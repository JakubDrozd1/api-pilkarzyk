namespace DataLibrary.IRepository
{
    public interface IDeleteGroupsRepository
    {
        Task DeleteGroupAsync(int groupId);
    }
}
