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
        public required String MESSAGE { get; set; }

    }
}
