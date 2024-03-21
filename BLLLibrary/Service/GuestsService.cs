using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class GuestsService(IUnitOfWork unitOfWork) : IGuestsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task AddGuestsAsync(GetGuestRequest getGuestRequest)
        {
            await _unitOfWork.CreateGuestsRepository.AddGuestsAsync(getGuestRequest);
        }

        public async Task DeleteGuestsAsync(int guestsId)
        {
            await _unitOfWork.DeleteGuestsRepository.DeleteGuestsAsync(guestsId);
        }

        public async Task<List<GUESTS?>> GetAllGuestFromMeetingAsync(int meetingId)
        {
            return await _unitOfWork.ReadGuestsRepository.GetAllGuestFromMeetingAsync(meetingId);
        }

        public async Task<GUESTS?> GetGuestByIdAsync(int guestId)
        {
            return await _unitOfWork.ReadGuestsRepository.GetGuestByIdAsync(guestId);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateGuestsAsync(GetGuestRequest getGuestRequest, int guestId)
        {
            await _unitOfWork.UpdateGuestsRepository.UpdateGuestsAsync(new GUESTS()
            {
                IDMEETING = getGuestRequest.IDMEETING,
                ID_GUEST = guestId,
                NAME = getGuestRequest.NAME
            });
        }
    }
}
