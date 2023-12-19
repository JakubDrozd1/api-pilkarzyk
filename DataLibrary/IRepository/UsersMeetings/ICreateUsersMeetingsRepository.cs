using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.UsersMeetings
{
    public interface ICreateUsersMeetingsRepository
    {
        Task AddUserToMeetingAsync(int idMeeting, int idUser, FbTransaction? transaction = null);
        Task AddUsersToMeetingAsync(GetUsersMeetingsRequest getUsersMeetingsRequest, FbTransaction? transaction = null);

    }
}
