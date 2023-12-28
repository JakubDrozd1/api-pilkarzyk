using DataLibrary.Entities;

namespace DataLibrary.IRepository.EmailSender
{
    public interface IReadEmailSender
    {
        Task<EMAIL_SENDER?> GetEmailDetailsAsync(string email);
    }
}
