using DataLibrary.Entities;

namespace BLLLibrary.IService
{
    public interface ITeamsService
    {
        Task<List<TEAMS?>> GetTeamByMeetingIdAsync(int meetingId);

    }
}
