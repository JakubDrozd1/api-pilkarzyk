using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetUpdateNotificationMessageRequest
    {
        [JsonPropertyName("Sended")]
        public bool SENDED { get; set; }
        public required string[] Column { get; set; }

    }
}
