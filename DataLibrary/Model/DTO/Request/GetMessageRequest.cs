﻿using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request
{
    public class GetMessageRequest
    {
        [JsonPropertyName("IdMeeting")]
        public int? IDMEETING { get; set; }

        [JsonPropertyName("IdUser")]
        public int? IDUSER { get; set; }

        [JsonPropertyName("Answer")]
        public string? ANSWER { get; set; }

        [JsonPropertyName("WaitingTime")]
        public DateTime? WAITING_TIME { get; set; }
    }
}
