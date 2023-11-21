namespace WebApi.Model.DTO.Request
{
    public class MeetingRequest
    {
        public DateTime DateMeeting { get; set; }
        public string? Place { get; set; }
        public string? Quantity { get; set; }
        public string? Description { get; set; }
        public int IdUser { get; set; }
        public int IdGroup { get; set; }
    }
}
