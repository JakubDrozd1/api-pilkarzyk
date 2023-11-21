using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface IUpdateMeetingsRepository
    {
        Task UpdateMeetingAsync(Meeting meeting);
    }
}
