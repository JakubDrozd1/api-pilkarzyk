using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IUpdateMessagesRepository
    {
        Task UpdateMessageAsync(Message message, FbTransaction? transaction = null);
    }
}
