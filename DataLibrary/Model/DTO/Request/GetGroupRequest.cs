using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request
{
    public class GetGroupRequest
    {
        [JsonPropertyName("Name")]
        public required string NAME { get; set; }
    }
}
