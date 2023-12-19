namespace DataLibrary.Entities
{
    public class USERS_MEETINGS
    {
        public int? ID_USER_MEETING {  get; set; }
        public required int IDUSER { get; set; }
        public required int IDMEETING { get; set; }
    }
}
