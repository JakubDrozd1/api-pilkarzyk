using DataLibrary.Entities;

namespace DataLibrary.IRepository.Guests
{
    public interface IUpdateGuestsRepository
    {
        Task UpdateGuestsAsync(GUESTS guest);
    }
}
