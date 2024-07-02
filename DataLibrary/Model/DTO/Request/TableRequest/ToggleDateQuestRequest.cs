using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class ToggleDateQuestRequest
    {
        [JsonPropertyName("IdUser")]
        public int?  IdUser { get; set; }

        [JsonPropertyName("IdMeeting")]
        public int? IdMeeting { get; set; }
    }
}
