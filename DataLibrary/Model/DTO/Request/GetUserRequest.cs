using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request
{
    public class GetUserRequest
    {
        [JsonPropertyName("Login")]
        public required string LOGIN { get; set; }

        [JsonPropertyName("Password")]
        public required string USER_PASSWORD { get; set; }

        [JsonPropertyName("Email")]
        public required string EMAIL { get; set; }

        [JsonPropertyName("Firstname")]
        public required string FIRSTNAME { get; set; }

        [JsonPropertyName("Surname")]
        public required string SURNAME { get; set; }

        [JsonPropertyName("PhoneNumber")]
        public int PHONE_NUMBER { get; set; }

        [JsonPropertyName("IsAdmin")]
        public bool IS_ADMIN { get; set; }

        [JsonPropertyName("Salt")]
        public string? SALT { get; set; }

        [JsonPropertyName("GroupCounter")]
        public int GROUP_COUNTER { get; set; }

    }
}
