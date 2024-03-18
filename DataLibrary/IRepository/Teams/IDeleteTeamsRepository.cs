namespace DataLibrary.IRepository.Teams
{
    public interface IDeleteTeamsRepository
    {
        Task DeleteTeamAsync(int teamId);
    }
}
