namespace WebApi.Model.DTO.Request
{
    public class MessageRequest
    {
        public int IdMeeting { get; set; }
        public int IdUser { get; set; }
        public string? Answer { get; set; }
    }
}
