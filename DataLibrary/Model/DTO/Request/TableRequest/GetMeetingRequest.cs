using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetMeetingRequest
    {
        [JsonPropertyName("DateMeeting")]
        public DateTime? DATE_MEETING { get; set; }

        [JsonPropertyName("Place")]
        public string? PLACE { get; set; }

        [JsonPropertyName("Quantity")]
        public int? QUANTITY { get; set; }

        [JsonPropertyName("Description")]
        public string? DESCRIPTION { get; set; }

        [JsonPropertyName("IdGroup")]
        public int? IDGROUP { get; set; }

        [JsonPropertyName("IdAuthor")]
        public int? IDAUTHOR { get; set; }

        [JsonPropertyName("IsIndependent")]
        public bool? IS_INDEPENDENT {  get; set; }

        [JsonPropertyName("WaitingTimeDecision")]
        public int? WAITING_TIME_DECISION {  get; set; }
    }
}
