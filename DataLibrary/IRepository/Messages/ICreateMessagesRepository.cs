using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface ICreateMessagesRepository
    {
        Task AddMessageAsync(MESSAGES message, FbTransaction? transaction = null);
    }
}
