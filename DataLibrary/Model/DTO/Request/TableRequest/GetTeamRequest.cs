using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetTeamRequest
    {
        [JsonPropertyName("IdMeeting")]
        [FromQuery(Name = "IdMeeting")]
        public int? IDMEETING { get; set; }

        [JsonPropertyName("Name")]
        [FromQuery(Name = "Name")]
        public required string NAME { get; set; }

        [JsonPropertyName("Color")]
        [FromQuery(Name = "Color")]
        public required string COLOR { get; set; }
    }
}
