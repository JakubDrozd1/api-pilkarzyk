using DataLibrary.Entities;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.UsersMeetings
{
    public interface ICreateUsersMeetingsRepository
    {
        Task AddUserToMeetingAsync(GetMeetingGroupsResponse meeting, USERS user);
    }
}
