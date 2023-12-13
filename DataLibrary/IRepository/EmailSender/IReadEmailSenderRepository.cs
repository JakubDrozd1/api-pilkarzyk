using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IReadEmailSender
    {
        Task<EMAIL_SENDER?> GetEmailDetailsAsync(string email, FbTransaction? transaction = null);
    }
}
