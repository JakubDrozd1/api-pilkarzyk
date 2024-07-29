using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Response
{
    public class GetAdResponse
    {
        [JsonPropertyName("id")]
        [JsonProperty(PropertyName = "id")]
        public required int Id { get; set; }

        [JsonPropertyName("content")]
        [JsonProperty(PropertyName = "content")]
        public required string Content { get; set; }

        [JsonPropertyName("url")]
        [JsonProperty(PropertyName = "url")]
        public required string Url { get; set; }

        [JsonPropertyName("color")]
        [JsonProperty(PropertyName = "color")]
        public required string Color { get; set; }

    }
}
