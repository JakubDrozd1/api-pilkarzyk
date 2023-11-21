using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.UoW;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.Service
{
    public class MeetingsService(IUnitOfWork unitOfWork) : IMeetingsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<Meeting>> GetAllMeetingsAsync()
        {
            return await _unitOfWork.ReadMeetingsRepository.GetAllMeetingsAsync();
        }

        public async Task<Meeting?> GetMeetingByIdAsync(int meetingId)
        {
            return await _unitOfWork.ReadMeetingsRepository.GetMeetingByIdAsync(meetingId);
        }

        public async Task AddMeetingAsync(MeetingRequest meetingRequest)
        {
            Meeting meeting = new()
            {
                DESCIPTION = meetingRequest.Description,
                QUANTITY = meetingRequest.Quantity,
                ID_GROUP = meetingRequest.IdGroup,
                ID_USER = meetingRequest.IdUser
            };
            await _unitOfWork.CreateMeetingsRepository.AddMeetingAsync(meeting);
        }

        public async Task UpdateMeetingAsync(MeetingRequest meetingRequest, int meetingId)
        {
            Meeting meeting = new()
            {
                ID_MEETING = meetingId,
                DESCIPTION = meetingRequest.Description,
                QUANTITY = meetingRequest.Quantity,
                ID_GROUP = meetingRequest.IdGroup,
                ID_USER = meetingRequest.IdUser
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
