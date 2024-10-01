namespace DataLibrary.Entities
{
    public class NOTIFICATION_MESSAGES
    {
        public int? ID_NOTIFICATION_MESSAGES { get; set; }
        public DateTime? DATE_SEND { get; set; }
        public int IDUSER { get; set; }
        public int? IDGROUP { get; set; }
        public int? IDMEETING { get; set; }
        public string? BODY { get; set; }
        public string? TITLE { get; set; }
    }
}
