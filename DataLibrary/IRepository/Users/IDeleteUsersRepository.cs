namespace DataLibrary.IRepository.Users
{
    public interface IDeleteUsersRepository
    {
        Task DeleteUserAsync(int userId);
    }
}
