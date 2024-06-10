using DataLibrary.Entities;

namespace DataLibrary.IRepository.DateQuest
{
    public interface IUpdateDateQuestsRepository
    {
        Task UpdateDateQuestAsync(DATE_QUESTS dateQuest);   
    }
}
