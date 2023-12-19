using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.EmailSender
{
    public interface IReadEmailSender
    {
        Task<EMAIL_SENDER?> GetEmailDetailsAsync(string email, FbTransaction? transaction = null);
    }
}
