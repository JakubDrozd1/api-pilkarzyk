﻿using System.ComponentModel.DataAnnotations;
using BLLLibrary.IService;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    //[Authorize]
    [Route("api/users-meetings")]
    [ApiController]
    public class UsersMeetingsController(IUsersMeetingsService usersMeetingsService) : ControllerBase
    {
        private readonly IUsersMeetingsService _usersMeetingsService = usersMeetingsService;

        [HttpPost("add", Name = "AddUserToMeetingAsync")]
        public async Task<IActionResult> AddUserToMeetingAsync([FromQuery, Required] int idMeeting, [FromQuery, Required] int idUser)
        {
            await _usersMeetingsService.AddUserToMeetingAsync(idMeeting, idUser);
            return Ok();
        }

        [HttpPost("adds", Name = "AddUsersToMeetingAsync")]
        public async Task<IActionResult> AddUsersToMeetingAsync([FromQuery, Required] GetUsersMeetingsRequest getUsersMeetingsRequest)
        {
            await _usersMeetingsService.AddUsersToMeetingAsync(getUsersMeetingsRequest);
            return Ok();
        }

        [HttpGet("all", Name = "GetListMeetingsUsersAsync")]
        public async Task<ActionResult<List<GetMeetingUsersResponse>>> GetListMeetingsUsersAsync([FromQuery] GetMeetingsUsersPaginationRequest getMeetingsUsersPaginationRequest)
        {
            var result = await _usersMeetingsService.GetListMeetingsUsersAsync(getMeetingsUsersPaginationRequest);
            return Ok(result);
        }

        [HttpGet(Name = "GetUserWithMeeting")]
        public async Task<ActionResult<GetGroupsUsersResponse?>> GetUserWithMeeting([FromQuery, Required] int meetingId, [FromQuery, Required] int userId)
        {
            var result = await _usersMeetingsService.GetUserWithMeeting(meetingId, userId);
            return Ok(result);
        }
    }
}
