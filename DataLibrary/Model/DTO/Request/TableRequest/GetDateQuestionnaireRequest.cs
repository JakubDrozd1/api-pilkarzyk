using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetDateQuestRequest
    {
        [JsonPropertyName("IdMeeting")]
        [FromQuery(Name = "IdMeeting")]
        public int? IDMEETING { get; set; }

        [JsonPropertyName("DateMeeting")]
        [FromQuery(Name = "DateMeeting")]
        public required DateTime DATE_MEETING { get; set; }
    }
}
