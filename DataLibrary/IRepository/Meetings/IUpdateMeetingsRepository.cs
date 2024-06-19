using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.Meetings
{
    public interface IUpdateMeetingsRepository
    {
        Task UpdateMeetingAsync(MEETINGS meeting);
        void UpdateMeeting(MEETINGS meeting);
        Task UpdateColumnMeetingAsync(GetUpdateMeetingRequest getUpdateMeetingRequest, int meetingId);
    }
}
