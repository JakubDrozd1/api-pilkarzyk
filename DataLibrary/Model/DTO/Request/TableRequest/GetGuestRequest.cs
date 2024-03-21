using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetGuestRequest
    {
        [JsonPropertyName("Name")]
        public string? NAME { get; set; }

        [JsonPropertyName("IdMeeting")]
        public int IDMEETING { get; set; }
    }
}
