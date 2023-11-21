namespace DataLibrary.IRepository
{
    public interface IDeleteMessagesRepository
    {
        Task DeleteMessageAsync(int messageId);
    }
}
