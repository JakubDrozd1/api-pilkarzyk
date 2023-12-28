using DataLibrary.Model.DTO.Request;

namespace DataLibrary.IRepository.Meetings
{
    public interface ICreateMeetingsRepository
    {
        Task AddMeetingAsync(GetMeetingRequest getMeetingRequest);
    }
}
