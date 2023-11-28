namespace DataLibrary.Entities
{
    public class ACCESS_TOKENS
    {
        public int? ID_TOKEN { get; set; }
        public required int IDUSER { get; set; }
        public required string ACCESS_TOKEN_VALUE { get; set; }
        public required DateTime DATE_EXPIRE { get; set; }
        public required string IDCLIENT { get; set; }
    }
}
