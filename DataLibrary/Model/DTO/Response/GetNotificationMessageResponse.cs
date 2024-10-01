namespace DataLibrary.Model.DTO.Response
{
    public class GetNotificationMessageResponse
    {
        public DateTime? DateSend { get; set; }
        public int IdUser { get; set; }
        public int? IdGroup { get; set; }
        public int? IdMeeting { get; set; }
        public string? Body { get; set; }
        public string? Title { get; set; }
    }
}
