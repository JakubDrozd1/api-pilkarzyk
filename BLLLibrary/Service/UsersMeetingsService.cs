using BLLLibrary.IService;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class UsersMeetingsService(IUnitOfWork unitOfWork) : IUsersMeetingsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<GetMeetingUsersResponse>> GetListMeetingsUsersAsync(GetMeetingsUsersPaginationRequest getMeetingsUsersPaginationRequest)
        {
            return await _unitOfWork.ReadUsersMeetingsRepository.GetListMeetingsUsersAsync(getMeetingsUsersPaginationRequest);
        }

        public async Task<GetMeetingUsersResponse?> GetUserWithMeeting(int meetingId, int userId)
        {
            return await _unitOfWork.ReadUsersMeetingsRepository.GetUserWithMeeting(meetingId, userId);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
