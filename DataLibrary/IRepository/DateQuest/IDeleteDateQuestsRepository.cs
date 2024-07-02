namespace DataLibrary.IRepository.DateQuest
{
    public interface IDeleteDateQuestsRepository
    {
        Task DeleteDateQuestAsync(int dateQuestId);
    }
}
