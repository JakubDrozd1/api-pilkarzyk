using DataLibrary.Entities;

namespace DataLibrary.IRepository.Teams
{
    public interface IUpdateTeamsRepository
    {
        Task UpdateTeamAsync(TEAMS team);
    }
}
