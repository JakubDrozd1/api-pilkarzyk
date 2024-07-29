using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class PostAdHistoryRequest
    {
        [JsonPropertyName("IdUser")]
        public int? IDUSER { get; set; }

        [JsonPropertyName("IdAd")]
        public int? IDAD { get; set; }

    }
}
