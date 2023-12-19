using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Response
{
    public class GetTokenResponse
    {
        [JsonPropertyName("access_token")]
        [JsonProperty(PropertyName = "access_token")]
        public required string AccessToken { get; set; }
        [JsonPropertyName("expires_in")]
        [JsonProperty(PropertyName = "expires_in")]
        public double ExpiresIn { get; set; }
        [JsonPropertyName("refresh_token")]
        [JsonProperty(PropertyName = "refresh_token")]
        public required string RefreshToken { get; set; }
        [JsonPropertyName("atoken_type")]
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; } = "Bearer";
    }
}
