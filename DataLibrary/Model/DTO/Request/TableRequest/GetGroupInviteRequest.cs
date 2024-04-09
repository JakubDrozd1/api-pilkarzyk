using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetGroupInviteRequest
    {
        [JsonPropertyName("IdGroup")]
        public required int IDGROUP { get; set; }

        [JsonPropertyName("IdUser")]
        public int? IDUSER { get; set; }

        [JsonPropertyName("IdAuthor")]
        public required int IDAUTHOR { get; set; }

        [JsonPropertyName("Email")]
        public string? EMAIL { get; set; }

        [JsonPropertyName("PhoneNumber")]
        public int? PHONE_NUMBER { get; set; }
    }
}
