using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;

namespace BLLLibrary.IService
{
    public interface ITeamsService
    {
        Task<List<TEAMS?>> GetTeamByMeetingIdAsync(int meetingId);
        Task AddTeamsAsync(GetTeamRequest getTeamRequest);
        Task UpdateTeamAsync(int teamId, GetTeamRequest getTeamRequest);
        Task DeleteTeamAsync(int teamId);
        Task SaveChangesAsync();

    }
}
