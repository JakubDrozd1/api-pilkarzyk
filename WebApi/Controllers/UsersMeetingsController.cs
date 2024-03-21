using System.ComponentModel.DataAnnotations;
using BLLLibrary.IService;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/users-meetings")]
    [ApiController]
    public class UsersMeetingsController(IUsersMeetingsService usersMeetingsService) : ControllerBase
    {
        private readonly IUsersMeetingsService _usersMeetingsService = usersMeetingsService;

        [HttpGet("all", Name = "GetListMeetingsUsers")]
        public async Task<ActionResult<List<GetMeetingUsersResponse>>> GetListMeetingsUsers([FromQuery] GetMeetingsUsersPaginationRequest getMeetingsUsersPaginationRequest)
        {
            try
            {
                var result = await _usersMeetingsService.GetListMeetingsUsersAsync(getMeetingsUsersPaginationRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet(Name = "GetUserWithMeeting")]
        public async Task<ActionResult<GetMeetingUsersResponse?>> GetUserWithMeeting([FromQuery, Required] int meetingId, [FromQuery, Required] int userId)
        {
            try
            {
                var result = await _usersMeetingsService.GetUserWithMeeting(meetingId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
