using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request
{
    public class GetTeamRequest
    {
        [JsonPropertyName("IdMeeting")]
        public int? IDMEETING { get; set; }

        [JsonPropertyName("Name")]
        public required string NAME { get; set; }

        [JsonPropertyName("Color")]
        public required string COLOR { get; set; }
    }
}
