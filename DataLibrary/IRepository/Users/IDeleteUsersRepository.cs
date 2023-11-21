namespace DataLibrary.IRepository
{
    public interface IDeleteUsersRepository
    {
        Task DeleteUserAsync(int userId);
    }
}
