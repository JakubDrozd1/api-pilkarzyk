namespace DataLibrary.Entities
{
    public class Message
    {
        public int? ID_MESSAGE { get; set; }
        public int? IDMEETING { get; set; }
        public int? IDUSER { get; set; }
        public string? ANSWER { get; set; }
        public DateTime? DATE_ADD { get; set; }
    }
}
