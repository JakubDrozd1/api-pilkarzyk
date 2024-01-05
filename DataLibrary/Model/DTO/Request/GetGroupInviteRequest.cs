﻿using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request
{
    public class GetGroupInviteRequest
    {
        [JsonPropertyName("IdGroup")]
        public required int IDGROUP { get; set; }

        [JsonPropertyName("IdUser")]
        public required int IDUSER { get; set; }

        [JsonPropertyName("IdAuthor")]
        public required int IDAUTHOR { get; set; }
    }
}