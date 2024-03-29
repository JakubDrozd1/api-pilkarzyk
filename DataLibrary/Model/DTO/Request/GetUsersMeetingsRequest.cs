﻿using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.Model.DTO.Request
{
    public class GetUsersMeetingsRequest
    {
        public required GetMeetingRequest Meeting { get; set; }
        public required GetMessageRequest Message { get; set; }
        public GetTeamRequest[]? Team { get; set; }
    }
}
