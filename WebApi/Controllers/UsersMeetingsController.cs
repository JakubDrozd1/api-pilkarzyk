using System.ComponentModel.DataAnnotations;
using BLLLibrary.IService;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
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

        [HttpPost("add", Name = "AddUsersToMeetingAsync")]
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
        public async Task<ActionResult<GetMeetingUsersResponse?>> GetUserWithMeeting([FromQuery, Required] int meetingId, [FromQuery, Required] int userId)
        {
            var result = await _usersMeetingsService.GetUserWithMeeting(meetingId, userId);
            return Ok(result);
        }
    }
}
