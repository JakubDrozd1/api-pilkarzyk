using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.DateQuest
{
    public interface ICreateDateQuestsRepository
    {
        Task AddDateQuestAsync(GetDateQuestRequest dateQuest);
    }
}
