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
    }
}
