namespace DataLibrary.IRepository
{
    public interface IDeleteMeetingsRepository
    {
        Task DeleteMeetingAsync(int meetingId);
    }
}
