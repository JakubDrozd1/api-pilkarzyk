using BLLLibrary.IService;
using DataLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.DTO.Request;

namespace WebApi.Controllers
{
    [Route("api/meetings")]
    [ApiController]
    public class MeetingsController(IMeetingsService meetingsService) : ControllerBase
    {
        private readonly IMeetingsService _meetingsService = meetingsService;

        [HttpGet]
        public async Task<ActionResult<List<Meeting>>> GetAllMeetings()
        {
            var meetings = await _meetingsService.GetAllMeetingsAsync();
            return Ok(meetings);
        }

        [HttpGet("{meetingId}")]
        public async Task<ActionResult<Meeting>> GetMeetingById(int meetingId)
        {
            var meeting = await _meetingsService.GetMeetingByIdAsync(meetingId);
            if (meeting == null)
            {
                return NotFound();
            }
            return Ok(meeting);
        }

        [HttpPost]
        public async Task<ActionResult> AddMeeting([FromBody] MeetingRequest meetingRequest)
        {
            try
            {
                await _meetingsService.AddMeetingAsync(meetingRequest);
                await _meetingsService.SaveChangesAsync();
                return Ok(meetingRequest);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }


        }

        [HttpPut("{meetingId}")]
        public async Task<ActionResult> UpdateMeeting(int meetingId, [FromBody] MeetingRequest meetingRequest)
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

        [HttpDelete("{meetingId}")]
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
