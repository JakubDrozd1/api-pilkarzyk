using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;

namespace BLLLibrary.IService
{
    public interface IDateQuestsService
    {
        Task<List<DATE_QUESTS?>> GetDateQuestByMeetingIdAsync(int meetingId);
        Task AddDateQuestAsync(GetDateQuestRequest getDateQuestionaryRequest);
        Task UpdateDateQuestAsync(int dateQuestionaryId, GetDateQuestRequest getDateQuestionaryRequest);
        Task DeleteDateQuestAsync(int dateQuestionaryId);
        Task SaveChangesAsync();

    }
}
