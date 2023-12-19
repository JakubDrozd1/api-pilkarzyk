namespace DataLibrary.Model.DTO.Request
{
    public class GetMessageRequest
    {
        public int? IdMeeting { get; set; }
        public int? IdUser { get; set; }
        public string? Answer { get; set; }
    }
}
