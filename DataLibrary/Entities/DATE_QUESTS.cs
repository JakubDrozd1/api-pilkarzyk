
namespace DataLibrary.Entities
{
    public class DATE_QUESTS
    {
        public int? ID_DATE_QUEST { get; set; }
        public int? IDMEETING { get; set; }
        public DateTime? DATE_MEETING { get; set; }

        public List<USERS_DATE_QUESTS> USERS_DATE_QUESTS { get; set; } = [];
    }
}
