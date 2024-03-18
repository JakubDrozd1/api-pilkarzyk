using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class TeamsService(IUnitOfWork unitOfWork) : ITeamsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public Task<List<TEAMS?>> GetTeamByMeetingIdAsync(int meetingId)
        {
            return _unitOfWork.ReadTeamsRepository.GetTeamByMeetingIdAsync(meetingId);
        }

        public async Task AddTeamsAsync(GetTeamRequest getTeamRequest)
        {
            await _unitOfWork.CreateTeamsRepository.AddTeamsAsync(getTeamRequest);
        }

        public async Task UpdateTeamAsync(int teamId, GetTeamRequest getTeamRequest)
        {
            var team = new TEAMS()
            {
                COLOR = getTeamRequest.COLOR,
                IDMEETING = getTeamRequest?.IDMEETING,
                ID_TEAM = teamId,
                NAME = getTeamRequest?.NAME ?? ""
            };
            await _unitOfWork.UpdateTeamsRepository.UpdateTeamAsync(team);
        }

        public async Task DeleteTeamAsync(int teamId)
        {
            await _unitOfWork.DeleteTeamsRepository.DeleteTeamAsync(teamId);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
