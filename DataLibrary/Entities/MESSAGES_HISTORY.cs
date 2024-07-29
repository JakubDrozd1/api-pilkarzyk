namespace DataLibrary.Entities
{
    public class MESSAGES_HISTORY
    {
        public int ID_MESSAGE_HISTORY { get; set; }
        public int IDMESSAGE { get; set; }
        public required string BEFORE_CHANGE { get; set; }
        public required string AFTER_CHANGE { get; set; }
        public DateTime DATE_CHANGE { get; set; }


    }
}
