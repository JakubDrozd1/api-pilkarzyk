using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Messages
{
    public interface ICreateMessagesRepository
    {
        Task AddMessageAsync(MESSAGES message, FbTransaction? transaction = null);
    }
}
