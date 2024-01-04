using BLLLibrary.IService;
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
            var meetings = await _meetingsService.GetAllMeetingsAsync(getMeetingsUsersGroupsPaginationRequest);
            return Ok(meetings);
        }

        [HttpGet("{meetingId}", Name = "GetMeetingById")]
        public async Task<ActionResult<MEETINGS>> GetMeetingById(int meetingId)
        {
            var meeting = await _meetingsService.GetMeetingByIdAsync(meetingId);
            if (meeting == null)
            {
                return NotFound();
            }
            return Ok(meeting);
        }

        [HttpPost(Name = "AddMeeting")]
        public async Task<ActionResult> AddMeeting([FromBody] GetMeetingRequest meetingRequest)
        {
            try
            {
                await _meetingsService.AddMeetingAsync(meetingRequest);
                await _meetingsService.SaveChangesAsync();
                return Ok(await _meetingsService.GetMeeting(meetingRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        [HttpPut("{meetingId}", Name = "UpdateMeeting")]
        public async Task<ActionResult> UpdateMeeting(int meetingId, [FromBody] GetMeetingRequest meetingRequest)
        {
            var existingMeeting = await _meetingsService.GetMeetingByIdAsync(meetingId);
            if (existingMeeting == null)
            {
                return NotFound();
            }
            try
            {
                await _meetingsService.UpdateMeetingAsync(meetingRequest, meetingId);
                await _meetingsService.SaveChangesAsync();
                return Ok(meetingRequest);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{meetingId}", Name = "DeleteMeeting")]
        public async Task<ActionResult> DeleteMeeting(int meetingId)
        {
            var existingMeeting = await _meetingsService.GetMeetingByIdAsync(meetingId);

            if (existingMeeting == null)
            {
                return NotFound();
            }
            try
            {
                await _meetingsService.DeleteMeetingAsync(meetingId);
                await _meetingsService.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
