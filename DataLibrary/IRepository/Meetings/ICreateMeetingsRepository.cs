using DataLibrary.Entities;

namespace DataLibrary.IRepository.Meetings
{
    public interface ICreateMeetingsRepository
    {
        Task AddMeetingAsync(Meeting meeting);
    }
}
