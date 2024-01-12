using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class MeetingsService(IUnitOfWork unitOfWork) : IMeetingsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<GetMeetingGroupsResponse>> GetAllMeetingsAsync(GetMeetingsGroupsPaginationRequest getMeetingsPaginationRequest)
        {
            return await _unitOfWork.ReadMeetingsRepository.GetAllMeetingsAsync(getMeetingsPaginationRequest);
        }

        public async Task<MEETINGS?> GetMeetingByIdAsync(int meetingId)
        {
            return await _unitOfWork.ReadMeetingsRepository.GetMeetingByIdAsync(meetingId);
        }

        public async Task<MEETINGS?> GetMeeting(GetMeetingRequest getMeetingRequest)
        {
            return await _unitOfWork.ReadMeetingsRepository.GetMeeting(getMeetingRequest);
        }

        public async Task AddMeetingAsync(GetMeetingRequest getMeetingRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var check = await _unitOfWork.ReadMeetingsRepository.GetMeeting(getMeetingRequest);
                if (check != null)
                {
                    throw new Exception("Event already exists");
                }
                await _unitOfWork.CreateMeetingsRepository.AddMeetingAsync(getMeetingRequest);
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                throw new Exception($"{ex.Message}");
            }

        }

        public async Task UpdateMeetingAsync(GetMeetingRequest getMeetingRequest, int meetingId)
        {
            MEETINGS meeting = new()
            {
                ID_MEETING = meetingId,
                DESCRIPTION = getMeetingRequest.DESCRIPTION,
                QUANTITY = getMeetingRequest.QUANTITY,
                DATE_MEETING = getMeetingRequest.DATE_MEETING,
                PLACE = getMeetingRequest.PLACE,
                IDGROUP = getMeetingRequest.IDGROUP,
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
