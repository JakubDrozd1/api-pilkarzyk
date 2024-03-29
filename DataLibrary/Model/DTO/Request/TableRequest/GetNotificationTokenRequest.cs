﻿using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetNotificationTokenRequest
    {

        [JsonPropertyName("Token")]
        public required string TOKEN { get; set; }

        [JsonPropertyName("IdUser")]
        public required int IDUSER { get; set; }
    }
}
