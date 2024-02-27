using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request
{
    public class GetResetPasswordRequest
    {
        [JsonPropertyName("IdUser")]
        public int? IDUSER { get; set; }
    }
}
