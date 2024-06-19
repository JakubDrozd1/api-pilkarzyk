using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IDateQuestsService
    {
        Task<List<DATE_QUESTS>> GetDateQuestByMeetingIdAsync(int meetingId);
        List<DATE_QUESTS> GetDateQuestByMeetingId(int meetingId);
        Task AddDateQuestAsync(GetDateQuestRequest getDateQuestionaryRequest);
        Task ToggleQuestAsync( int dateQuestId ,ToggleDateQuestRequest ToggleDateQuestRequest);
        Task UpdateDateQuestAsync(int dateQuestionaryId, GetDateQuestRequest getDateQuestionaryRequest);
        Task DeleteDateQuestAsync(int dateQuestionaryId);
        Task SaveChangesAsync();

    }
}
