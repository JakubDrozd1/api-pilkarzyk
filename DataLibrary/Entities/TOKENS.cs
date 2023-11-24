namespace DataLibrary.Entities
{
    public class TOKENS
    {
        public required int ID_TOKEN { get; set; }
        public required int IDUSER { get; set; }
        public required string TOKEN_VALUE { get; set; }
        public required DateTime DATE_EXPIRES { get; set; }
    }
}
