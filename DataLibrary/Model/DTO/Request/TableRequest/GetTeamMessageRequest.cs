using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetTeamMessageRequest
    {
        [JsonPropertyName("IdMeeting")]
        public int? IDMEETING { get; set; }

        [JsonPropertyName("IdUser")]
        public int? IDUSER { get; set; }

        [JsonPropertyName("IdTeam")]
        public int? IDTEAM { get; set; }
    }
}
