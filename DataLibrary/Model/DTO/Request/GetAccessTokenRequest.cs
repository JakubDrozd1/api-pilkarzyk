namespace DataLibrary.Model.DTO.Request
{
    public class GetAccessTokenRequest
    {
        public required string ACCESS_TOKEN_VALUE { get; set; }
        public required int IDUSER { get; set; }

        public required DateTime DATE_EXPIRE { get; set; }
        public required string IDCLIENT { get; set; }
    }
}
