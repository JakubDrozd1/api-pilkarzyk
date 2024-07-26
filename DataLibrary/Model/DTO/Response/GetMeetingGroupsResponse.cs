namespace DataLibrary.Model.DTO.Response
{
    public class GetMeetingGroupsResponse
    {
        public string? Name { get; set; }
        public DateTime? DateMeeting { get; set; }
        public string? Place { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        public int? IdMeeting { get; set; }
        public int? IdAuthor { get; set; }
        public int? IdGroup { get; set; }
        public bool? IsIndependent { get; set; }
        public int? IdMessage { get; set; }
        public int? WaitingTimeDecision { get; set; }
        public DateTime? DateQuestEnd { get; set; }
        public bool? DateQuestOpen { get; set; }
        public bool? IsQuest { get; set; }
        public int? MaxGiveMeTime { get; set; }
        public int? ReminderMessagesTime { get; set; }
        public DateTime? LastReminderMessagesTime { get; set; }
        public bool? Canceled { get; set; }

    }
}
