namespace DataLibrary.Entities
{
    public class MEETINGS
    {
        public int? ID_MEETING { get; set; }
        public DateTime? DATE_MEETING { get; set; }
        public string? PLACE { get; set; }
        public int? QUANTITY { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? IDGROUP { get; set; }
        public int? IDAUTHOR { get; set; }
        public bool? IS_INDEPENDENT { get; set; }
        public int? WAITING_TIME_DECISION { get; set; }
        public bool? IS_QUEST { get; set; }
        public bool? DATE_QUEST_OPEN { get; set; }
        public DateTime? DATE_QUEST_END { get; set; }
        public int? MAX_GIVE_ME_TIME { get; set; }
        public int? REMINDER_MESSAGES_TIME { get; set; }
        public DateTime? LAST_REMINDER_MESSAGES_TIME { get; set; }

    }
}
