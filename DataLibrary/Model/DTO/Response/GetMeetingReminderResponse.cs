namespace DataLibrary.Model.DTO.Response
{
    public class GetMeetingReminderResponse
    {
        public DateTime? DateMeeting { get; set; }
        public string? Place { get; set; }
        public string? Description { get; set; }
        public int? IdMeeting { get; set; }
        public bool? IsQuest { get; set; }
        public int? ReminderMessagesTime { get; set; }
        public DateTime? LastReminderMessagesTime { get; set; }
        public int? IdUser { get; set; }
        public int? WaitingTimeDecision { get; set; }
        public int? Quantity { get; set; }
        public int? IdGroup { get; set; }
        public int? IdAuthor { get; set; }
        public bool? IsIndependent { get; set; }

    }
}
