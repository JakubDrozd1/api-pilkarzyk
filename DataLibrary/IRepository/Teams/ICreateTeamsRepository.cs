using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.Teams
{
    public interface ICreateTeamsRepository
    {
        Task AddTeamsAsync(GetTeamRequest user);
    }
}
