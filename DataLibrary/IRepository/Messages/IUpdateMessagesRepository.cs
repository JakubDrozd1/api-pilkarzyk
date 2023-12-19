using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Messages
{
    public interface IUpdateMessagesRepository
    {
        Task UpdateMessageAsync(MESSAGES message, FbTransaction? transaction = null);
    }
}
