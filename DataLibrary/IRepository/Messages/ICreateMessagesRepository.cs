using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface ICreateMessagesRepository
    {
        Task AddMessageAsync(Message message, FbTransaction? transaction = null);
    }
}
