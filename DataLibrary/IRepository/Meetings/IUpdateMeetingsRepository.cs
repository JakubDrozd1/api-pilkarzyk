using DataLibrary.Entities;

namespace DataLibrary.IRepository.Meetings
{
    public interface IUpdateMeetingsRepository
    {
        Task UpdateMeetingAsync(Meeting meeting);
    }
}
