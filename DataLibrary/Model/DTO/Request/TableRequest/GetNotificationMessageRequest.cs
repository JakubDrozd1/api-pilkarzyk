using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetNotificationMessageRequest
    {
        [JsonPropertyName("IdUser")]
        public required int IDUSER { get; set; }

        [JsonPropertyName("IdGroup")]
        public int? IDGROUP { get; set; }

        [JsonPropertyName("IdMeeting")]
        public int? IDMEETING { get; set; }

        [JsonPropertyName("DateSend")]
        public required DateTime DATE_SEND { get; set; }

        [JsonPropertyName("Body")]
        public required string BODY { get; set; }
        
        [JsonPropertyName("Title")]
        public required string TITLE { get; set; }

    }
}
