using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IUpdateMessagesRepository
    {
        Task UpdateMessageAsync(MESSAGES message, FbTransaction? transaction = null);
    }
}
