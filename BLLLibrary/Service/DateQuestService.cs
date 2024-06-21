using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class DateQuestService(IUnitOfWork unitOfWork) : IDateQuestsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<DATE_QUESTS>> GetDateQuestByMeetingIdAsync(int meetingId)
        {
            return  await _unitOfWork.ReadDateQuestsRepository.GetDateQuestsByMeetingIdAsync(meetingId);
        }

        public async Task AddDateQuestAsync(GetDateQuestRequest getDateQuestRequest)
        {
            await _unitOfWork.CreateDateQuestsRepository.AddDateQuestAsync(getDateQuestRequest);
        }
        
        public async Task ToggleQuestAsync(int dateQuestId,  ToggleDateQuestRequest toggleDateQuestRequest)
        {
            await _unitOfWork.ToggleDateQuestsRepository.ToggleDateQuests(dateQuestId, toggleDateQuestRequest);
        }



        public async Task UpdateDateQuestAsync(int dateQuestionaryId, GetDateQuestRequest getDateQuestionaryRequest)
        {
            var dateQuestionary = new DATE_QUESTS()
            {
                IDMEETING = getDateQuestionaryRequest?.IDMEETING,
                ID_DATE_QUEST = dateQuestionaryId,
                DATE_MEETING = getDateQuestionaryRequest?.DATE_MEETING
            };
            await _unitOfWork.UpdateDateQuestsRepository.UpdateDateQuestAsync(dateQuestionary);
        }

        public async Task DeleteDateQuestAsync(int dateQuestId)
        {
            await _unitOfWork.DeleteDateQuestsRepository.DeleteDateQuestAsync(dateQuestId);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
