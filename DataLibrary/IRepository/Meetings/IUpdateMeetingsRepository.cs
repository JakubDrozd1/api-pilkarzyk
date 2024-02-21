using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;

namespace DataLibrary.IRepository.Meetings
{
    public interface IUpdateMeetingsRepository
    {
        Task UpdateMeetingAsync(MEETINGS meeting);
        Task UpdateColumnMeetingAsync(GetUpdateMeetingRequest getUpdateMeetingRequest, int meetingId);
    }
}
