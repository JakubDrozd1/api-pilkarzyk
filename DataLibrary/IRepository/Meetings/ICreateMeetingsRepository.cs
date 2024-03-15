using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.Meetings
{
    public interface ICreateMeetingsRepository
    {
        Task AddMeetingAsync(GetMeetingRequest getMeetingRequest);
    }
}
