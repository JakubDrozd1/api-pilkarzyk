namespace DataLibrary.Entities
{
    public class Meeting
    {
        public int? ID_MEETING { get; set; }
        public DateTime? DATE_MEETING { get; set; }
        public string? PLACE { get; set; }
        public string? QUANTITY { get; set; }
        public string? DESCIPTION { get; set; }
        public int? ID_USER { get; set; }
        public int? ID_GROUP { get; set; }
    }
}
