using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request
{
    public class GetUserGroupRequest
    {
        [JsonPropertyName("IdUser")]
        public required int IDUSER { get; set; }

        [JsonPropertyName("IdGroup")]
        public required int IDGROUP { get; set; }

        [JsonPropertyName("AccountType")]
        public int? ACCOUNT_TYPE { get; set; }
    }
}
