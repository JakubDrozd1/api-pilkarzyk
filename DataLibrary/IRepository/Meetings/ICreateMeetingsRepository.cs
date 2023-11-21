using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface ICreateMeetingsRepository
    {
        Task AddMeetingAsync(Meeting meeting);
    }
}
