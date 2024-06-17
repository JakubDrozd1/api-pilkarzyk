using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.DateQuest
{
    public interface IToggleDateQuestsRepository
    {
        Task ToggleDateQuests(int dateQuestId, ToggleDateQuestRequest dateQuest);   
    }
}
