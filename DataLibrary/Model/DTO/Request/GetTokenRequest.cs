namespace DataLibrary.Model.DTO
{
    public class GetTokenRequest
    {
        public required string Grant_type { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? Client_id { get; set; }
        public string? Client_secret { get; set; }
        public string? Refresh_token { get; set; }
        public string? Scope { get; set; }
    }
}
