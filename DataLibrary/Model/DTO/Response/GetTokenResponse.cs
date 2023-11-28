namespace DataLibrary.Model.DTO
{
    public class GetTokenResponse
    {
        public required string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public required string RefreshToken { get; set; }
        public string TokenType { get; } = "Bearer";
    }
}
