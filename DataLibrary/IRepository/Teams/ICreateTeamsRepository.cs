using DataLibrary.Model.DTO.Request;

namespace DataLibrary.IRepository.Teams
{
    public interface ICreateTeamsRepository
    {
        Task AddTeamsAsync(GetTeamRequest user);
    }
}
