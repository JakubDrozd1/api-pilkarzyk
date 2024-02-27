using BLLLibrary.IService;
using BLLLibrary.Service;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/meetings")]
    [ApiController]
    public class MeetingsController(IMeetingsService meetingsService) : ControllerBase
    {
        private readonly IMeetingsService _meetingsService = meetingsService;

        [HttpGet(Name = "GetAllMeetings")]
        public async Task<ActionResult<List<GetMeetingGroupsResponse>>> GetAllMeetings([FromQuery] GetMeetingsGroupsPaginationRequest getMeetingsUsersGroupsPaginationRequest)
        {
            try
            {
                var meetings = await _meetingsService.GetAllMeetingsAsync(getMeetingsUsersGroupsPaginationRequest);
                return Ok(meetings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{meetingId}", Name = "GetMeetingById")]
        public async Task<ActionResult<GetMeetingGroupsResponse>> GetMeetingById(int meetingId)
        {
            try
            {
                var meeting = await _meetingsService.GetMeetingByIdAsync(meetingId);
                if (meeting == null)
                {
                    return NotFound();
                }
                return Ok(meeting);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost(Name = "AddMeeting")]
        public async Task<ActionResult> AddMeeting([FromBody] GetUsersMeetingsRequest meetingRequest)
        {
            try
            {
                await _meetingsService.AddMeetingAsync(meetingRequest);
                await _meetingsService.SaveChangesAsync();
                return Ok(await _meetingsService.GetMeeting(meetingRequest.Meeting));
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut("{meetingId}", Name = "UpdateMeeting")]
        public async Task<ActionResult> UpdateMeeting(int meetingId, [FromBody] GetMeetingRequest meetingRequest)
        {
            try
            {
                var existingMeeting = await _meetingsService.GetMeetingByIdAsync(meetingId);
                if (existingMeeting == null)
                {
                    return NotFound();
                }
                await _meetingsService.UpdateMeetingAsync(meetingRequest, meetingId);
                await _meetingsService.SaveChangesAsync();
                return Ok(meetingRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("{meetingId}", Name = "DeleteMeeting")]
        public async Task<ActionResult> DeleteMeeting(int meetingId)
        {
            try
            {
                var existingMeeting = await _meetingsService.GetMeetingByIdAsync(meetingId);
                if (existingMeeting == null)
                {
                    return NotFound();
                }
                await _meetingsService.DeleteMeetingAsync(meetingId);
                await _meetingsService.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut("column-{meetingId}", Name = "UpdateColumnMeeting")]
        public async Task<ActionResult> UpdateColumnMeeting([FromBody] GetUpdateMeetingRequest getUpdateMeetingRequest, int meetingId)
        {
            try
            {
                await _meetingsService.UpdateColumnMeetingAsync(getUpdateMeetingRequest, meetingId);
                await _meetingsService.SaveChangesAsync();
                return Ok(getUpdateMeetingRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
