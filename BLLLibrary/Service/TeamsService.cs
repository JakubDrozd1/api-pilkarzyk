using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
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

        public async Task BulkUpdateTeamsMeeting(GetUpdateBulkTeamRequest getUpdateBulkTeamRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var oldTeams = await _unitOfWork.ReadTeamsRepository.GetTeamByMeetingIdAsync(getUpdateBulkTeamRequest.IdMeeting);
                var newTeams = getUpdateBulkTeamRequest.Teams;
                foreach (var team in newTeams)
                {
                    if (oldTeams.Count > 0)
                    {
                        if (oldTeams.Any(o => o?.ID_TEAM == team.IdTeam))
                        {
                            await _unitOfWork.UpdateTeamsRepository.UpdateTeamAsync(new TEAMS()
                            {
                                COLOR = team.Color,
                                IDMEETING = getUpdateBulkTeamRequest.IdMeeting,
                                ID_TEAM = team.IdTeam,
                                NAME = team.Name
                            });
                            oldTeams = oldTeams.Where(o => o?.ID_TEAM != team.IdTeam).ToList();
                        }
                        else
                        {
                            await _unitOfWork.CreateTeamsRepository.AddTeamsAsync(new GetTeamRequest()
                            {
                                COLOR = team.Color,
                                IDMEETING = getUpdateBulkTeamRequest.IdMeeting,
                                NAME = team.Name
                            });
                        }
                    }
                    else
                    {
                        await _unitOfWork.CreateTeamsRepository.AddTeamsAsync(new GetTeamRequest()
                        {
                            COLOR = team.Color,
                            IDMEETING = getUpdateBulkTeamRequest.IdMeeting,
                            NAME = team.Name
                        });
                    }
                }
                if (oldTeams.Count > 0)
                {
                    foreach (var team in oldTeams)
                    {
                        await _unitOfWork.DeleteTeamsRepository.DeleteTeamAsync(team?.ID_TEAM ?? throw new Exception("Team is null"));
                    }
                }
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
