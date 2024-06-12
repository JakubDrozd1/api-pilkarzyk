using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Response
{
    public class GetAdResponse
    {
        [JsonPropertyName("content")]
        [JsonProperty(PropertyName = "content")]
        public required string Content { get; set; }

        [JsonPropertyName("url")]
        [JsonProperty(PropertyName = "url")]
        public required string Url { get; set; }

    }
}
