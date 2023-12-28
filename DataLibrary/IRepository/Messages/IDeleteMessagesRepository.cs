namespace DataLibrary.IRepository.Messages
{
    public interface IDeleteMessagesRepository
    {
        Task DeleteMessageAsync(int messageId);
    }
}
