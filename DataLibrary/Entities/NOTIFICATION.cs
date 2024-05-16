namespace DataLibrary.Entities
{
    public class NOTIFICATION
    {
        public int ID_NOTIFICATION { get; set; }
        public required int IDUSER { get; set; }
        public bool MEETING_NOTIFICATION { get; set; }
        public bool GROUP_INV_NOTIFICATION { get; set; }
        public bool MEETING_ORGANIZER_NOTIFICATION { get; set; }
        public bool TEAM_NOTIFICATION { get; set; }
        public bool UPDATE_MEETING_NOTIFICATION { get; set; }
        public bool TEAM_ORGANIZER_NOTIFICATION { get; set; }
        public bool GROUP_ADD_NOTIFICATION { get; set; }
        public bool MEETING_CANCEL_NOTIFICATION { get; set; }
    }
}