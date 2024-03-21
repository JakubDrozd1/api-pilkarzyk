using DataLibrary.Entities;

namespace DataLibrary.IRepository.Guests
{
    public interface IDeleteGuestsRepository
    {
        Task DeleteGuestsAsync(int guestsId);  
    }
}
