﻿using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetGroupRequest
    {
        [JsonPropertyName("Name")]
        public required string NAME { get; set; }
    }
}
