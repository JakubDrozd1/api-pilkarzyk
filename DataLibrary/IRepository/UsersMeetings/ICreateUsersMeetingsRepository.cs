using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;

namespace DataLibrary.IRepository.UsersMeetings
{
    public interface ICreateUsersMeetingsRepository
    {
        Task AddUserToMeetingAsync(MEETINGS meeting, USERS user);
        Task SendNotification(GetUsersMeetingsRequest getUsersMeetingsRequest);

    }
}
