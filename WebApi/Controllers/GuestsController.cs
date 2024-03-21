using BLLLibrary.IService;
using BLLLibrary.Service;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Request;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/guests")]
    [ApiController]
    public class GuestsController(IGuestsService guestsService) : ControllerBase
    {
        private readonly IGuestsService _guestsService = guestsService;

        [HttpGet(Name = "GetAllGuestFromMeeting")]

        public async Task<ActionResult<List<GUESTS>>> GetAllGuestFromMeeting(int meetingId)
        {
            try
            {
                var guests = await _guestsService.GetAllGuestFromMeetingAsync(meetingId);
                return Ok(guests);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{guestId}", Name = "GetGuestById")]
        public async Task<ActionResult<GROUPS>> GetGuestById(int guestId)
        {
            try
            {
                var group = await _guestsService.GetGuestByIdAsync(guestId);
                if (group == null)
                {
                    return NotFound();
                }
                return Ok(group);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPost(Name = "AddGuests")]
        public async Task<ActionResult> AddGuests([FromBody] GetGuestRequest getGuestRequest)
        {
            try
            {
                await _guestsService.AddGuestsAsync(getGuestRequest);
                await _guestsService.SaveChangesAsync();
                return Ok(getGuestRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{guestId}", Name = "UpdateGuests")]
        public async Task<ActionResult> UpdateGuests(int guestId, [FromBody] GetGuestRequest getGuestRequest)
        {
            try
            {
                var existinGuest = await _guestsService.GetGuestByIdAsync(guestId);
                if (existinGuest == null)
                {
                    return NotFound();
                }
                await _guestsService.UpdateGuestsAsync(getGuestRequest, guestId);
                await _guestsService.SaveChangesAsync();
                return Ok(getGuestRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{guestsId}", Name = "DeleteGuests")]
        public async Task<ActionResult> DeleteGuests(int guestsId)
        {
            try
            {
                var existingGroup = await _guestsService.GetGuestByIdAsync(guestsId);
                if (existingGroup == null)
                {
                    return NotFound();
                }
                await _guestsService.DeleteGuestsAsync(guestsId);
                await _guestsService.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
