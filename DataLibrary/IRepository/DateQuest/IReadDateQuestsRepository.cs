using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.DateQuest
{
    public interface IReadDateQuestsRepository
    {
        Task<DATE_QUESTS?> GetDateQuestByIdAsync(int dateQuestId);
        Task<List<DATE_QUESTS>> GetDateQuestsByMeetingIdAsync(int meetingId);
        List<DATE_QUESTS> GetDateQuestByMeetingId(int meetingId);

    }
}
