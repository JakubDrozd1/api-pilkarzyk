using DataLibrary.Entities;

namespace DataLibrary.IRepository.Teams
{
    public interface IReadTeamsRepository
    {
        Task<List<TEAMS?>> GetTeamByMeetingIdAsync(int meetingId);

    }
}
