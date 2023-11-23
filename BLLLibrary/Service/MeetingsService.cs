using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.Service
{
    public class MeetingsService(IUnitOfWork unitOfWork) : IMeetingsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<GetMeetingUsersGroupsResponse>> GetAllMeetingsAsync(GetMeetingsUsersGroupsPaginationRequest getMeetingsPaginationRequest)
        {
            return await _unitOfWork.ReadMeetingsRepository.GetAllMeetingsAsync(getMeetingsPaginationRequest);
        }

        public async Task<MEETINGS?> GetMeetingByIdAsync(int meetingId)
        {
            return await _unitOfWork.ReadMeetingsRepository.GetMeetingByIdAsync(meetingId);
        }

        public async Task AddMeetingAsync(GetMeetingRequest meetingRequest)
        {
            MEETINGS meeting = new()
            {
                DESCRIPTION = meetingRequest.Description,
                QUANTITY = meetingRequest.Quantity,
                DATE_MEETING = meetingRequest.DateMeeting,
                PLACE = meetingRequest.Place,
                IDGROUP = meetingRequest.IdGroup,
                IDUSER = meetingRequest.IdUser
            };
            await _unitOfWork.CreateMeetingsRepository.AddMeetingAsync(meeting);
        }

        public async Task UpdateMeetingAsync(GetMeetingRequest meetingRequest, int meetingId)
        {
            MEETINGS meeting = new()
            {
                ID_MEETING = meetingId,
                DESCRIPTION = meetingRequest.Description,
                QUANTITY = meetingRequest.Quantity,
                DATE_MEETING = meetingRequest.DateMeeting,
                PLACE = meetingRequest.Place,
                IDGROUP = meetingRequest.IdGroup,
                IDUSER = meetingRequest.IdUser
            };
            await _unitOfWork.UpdateMeetingsRepository.UpdateMeetingAsync(meeting);
        }

        public async Task DeleteMeetingAsync(int meetingId)
        {
            await _unitOfWork.DeleteMeetingsRepository.DeleteMeetingAsync(meetingId);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
