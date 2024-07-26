using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetUpdateMeetingRequest
    {
        [JsonPropertyName("DateMeeting")]
        public DateTime? DATE_MEETING { get; set; }

        [JsonPropertyName("Place")]
        public string? PLACE { get; set; }

        [JsonPropertyName("Quantity")]
        public int? QUANTITY { get; set; }

        [JsonPropertyName("Description")]
        public string? DESCRIPTION { get; set; }

        [JsonPropertyName("IsIndependent")]
        public bool? IS_INDEPENDENT { get; set; }

        [JsonPropertyName("WaitingTimeDecision")]
        public int? WAITING_TIME_DECISION { get; set; }

        [JsonPropertyName("MaxGiveMeTime")]
        public int? MAX_GIVE_ME_TIME { get; set; }

        [JsonPropertyName("ReminderMessagesTime")]
        public int? REMINDER_MESSAGES_TIME { get; set; }
        public required string[] Column { get; set; }

        [JsonPropertyName("Canceled")]
        public bool? CANCELED { get; set; }

    }
}
