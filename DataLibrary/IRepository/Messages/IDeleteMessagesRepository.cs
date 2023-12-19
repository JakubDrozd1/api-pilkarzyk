using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Messages
{
    public interface IDeleteMessagesRepository
    {
        Task DeleteMessageAsync(int messageId, FbTransaction? transaction = null);
    }
}
