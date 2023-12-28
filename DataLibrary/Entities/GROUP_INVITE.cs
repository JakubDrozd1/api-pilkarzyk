namespace DataLibrary.Entities
{
    public class GROUP_INVITE
    {
        public int? ID_GROUP_INVITE { get; set; }
        public required int IDGROUP { get; set; }
        public required int IDUSER { get; set; }
        public required int IDAUTHOR { get; set; }
        public DateTime? DATE_ADD { get; set; }
    }
}
