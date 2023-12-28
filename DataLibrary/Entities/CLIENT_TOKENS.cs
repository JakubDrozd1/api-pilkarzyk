namespace DataLibrary.Entities
{
    public class CLIENT_TOKENS
    {
        public required string ID_CLIENT { get; set; }
        public required string CLIENT_SECRET { get; set; }
        public required string GRANT_TYPE { get; set; }
        public int IDUSER { get; set; }
        public int? TOKEN_TIME { get; set; }
        public int? REFRESH_TOKEN_TIME { get; set; }
    }
}
