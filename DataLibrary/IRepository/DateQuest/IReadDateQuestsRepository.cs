using DataLibrary.Entities;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.DateQuest
{
    public interface IReadDateQuestsRepository
    {
        Task<DATE_QUESTS?> GetDateQuestByIdAsync(int dateQuestId);
        Task<List<GetDateQuestsResponse>> GetDateQuestsByMeetingIdAsync(int meetingId);
    }
}
