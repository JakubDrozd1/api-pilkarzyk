namespace DataLibrary.Model.DTO.Request
{
    public class GetUsersMeetingsRequest
    {
        public required int[] IdUsers { get; set; }
        public required int IdMeeting { get; set; }
    }
}
