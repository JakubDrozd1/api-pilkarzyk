namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetRefreshTokenRequest
    {
        public required DateTime DATE_EXPIRE { get; set; }
        public required string REFRESH_TOKEN_VALUE { get; set; }
        public required int IDUSER { get; set; }
        public required string IDCLIENT { get; set; }
    }
}
