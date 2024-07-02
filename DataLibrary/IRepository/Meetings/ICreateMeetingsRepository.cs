using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.Meetings
{
    public interface ICreateMeetingsRepository
    {
        Task<int> AddMeetingAsync(GetMeetingRequest getMeetingRequest);
    }
}
