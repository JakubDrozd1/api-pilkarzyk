using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IDeleteMessagesRepository
    {
        Task DeleteMessageAsync(int messageId, FbTransaction? transaction = null);
    }
}
