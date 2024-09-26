namespace DataLibrary.Entities
{
    public class CHAT_MESSAGES
    {
        public int? ID_CHAT_MESSAGES { get; set; }
        public DateTime? DATE_SEND { get; set; }
        public int? IDUSER { get; set; }
        public int? IDMEETING { get; set; }
        public string? CHAT_MESSAGE { get; set; }
    }
}
