
using System.Text.Json.Serialization;
using DataLibrary.Entities;
using DataLibrary.Helper;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetUpdateUserRequest
    {
        [JsonPropertyName("Login")]
        public string? LOGIN { get; set; }

        [JsonPropertyName("UserPassword")]
        public string? USER_PASSWORD { get; set; }

        [JsonPropertyName("Email")]
        public string? EMAIL { get; set; }

        [JsonPropertyName("Firstname")]
        public string? FIRSTNAME { get; set; }

        [JsonPropertyName("Surname")]
        public string? SURNAME { get; set; }

        [JsonPropertyName("PhoneNumber")]
        public int? PHONE_NUMBER { get; set; }

        [JsonPropertyName("GroupCounter")]
        public int? GROUP_COUNTER { get; set; }

        [JsonPropertyName("Avatar")]
        [JsonConverter(typeof(JsonToByteArrayConverter))]
        public byte[]? AVATAR { get; set; }
        public required string[] Column { get; set; }
    }
}
