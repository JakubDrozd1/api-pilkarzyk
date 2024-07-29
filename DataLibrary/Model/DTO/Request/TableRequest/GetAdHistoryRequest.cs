using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetAdHistoryRequest
    {
        [JsonPropertyName("IdUser")]
        public int? IDUSER { get; set; }

        [JsonPropertyName("IdAd")]
        public int? IDAD { get; set; }

        [JsonPropertyName("DateClick")]
        public DateTime? DATE_CLICK { get; set; }
    }
}
