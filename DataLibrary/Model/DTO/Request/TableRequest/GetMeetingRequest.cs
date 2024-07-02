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

        [JsonPropertyName("DateQuestOpen")]
        public bool? DATE_QUEST_OPEN { get; set; }

        [JsonPropertyName("IsQuest")]
        public bool? IS_QUEST { get; set; }

        [JsonPropertyName("DateQuestEnd")]
        public DateTime? DATE_QUEST_END { get; set; }

        [JsonPropertyName("MaxGiveMeTime")]
        public int? MAX_GIVE_ME_TIME { get; set; }
    }
}
