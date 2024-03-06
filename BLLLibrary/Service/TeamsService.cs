using BLLLibrary.IService;
using DataLibrary.Entities;
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
    }
}
