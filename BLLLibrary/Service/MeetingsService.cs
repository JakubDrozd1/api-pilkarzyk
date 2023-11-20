using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    internal class MeetingsService(IUnitOfWork unitOfWork) : IMeetingsService
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

        public async Task AddMeetingAsync(Meeting meeting)
        {
            await _unitOfWork.CreateMeetingsRepository.AddMeetingAsync(meeting);
        }

        public async Task UpdateMeetingAsync(Meeting meeting)
        {
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
