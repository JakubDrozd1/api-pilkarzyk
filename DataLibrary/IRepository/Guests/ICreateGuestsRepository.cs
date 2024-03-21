using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.Guests
{
    public interface ICreateGuestsRepository
    {
        Task AddGuestsAsync(GetGuestRequest getGuestRequest);

    }
}
