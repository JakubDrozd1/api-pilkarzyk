using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetMessageHistoryRequest
    {
        [JsonPropertyName("IdMessage")]
        public int? IDMESSAGE { get; set; }

        [JsonPropertyName("BeforeChange")]
        public string? BEFORE_CHANGE { get; set; }

        [JsonPropertyName("AfterChange")]
        public string? AFTER_CHANGE { get; set; }

        [JsonPropertyName("DateChange")]
        public DateTime? DATE_CHANGE { get; set; }
    }
}
