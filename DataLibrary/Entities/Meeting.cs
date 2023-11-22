namespace DataLibrary.Entities
{
    public class Meeting
    {
        public int? ID_MEETING { get; set; }
        public DateTime? DATE_MEETING { get; set; }
        public string? PLACE { get; set; }
        public string? QUANTITY { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? IDUSER { get; set; }
        public int? IDGROUP { get; set; }
    }
}
