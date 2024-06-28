using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/date-quest")]
    [ApiController]
    [Authorize]
    public class DateQuestController(IDateQuestsService dateQuestsService) : ControllerBase
    {
        private readonly IDateQuestsService _dateQuestsService = dateQuestsService;

        [HttpGet("{meetingId}", Name = "GetAllDateQuestFromMeeting")]
        public async Task<ActionResult<List<DATE_QUESTS>>> GetAllDateQuestFromMeeting(int meetingId)
        {
            try
            {
                var dateQuests = await _dateQuestsService.GetDateQuestByMeetingIdAsync(meetingId);
                return Ok(dateQuests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost(Name = "AddDateQuest")]
        public async Task<ActionResult> AddDateQuest([FromQuery] GetDateQuestRequest getDateQuestRequest)
        {
            try
            {
                await _dateQuestsService.AddDateQuestAsync(getDateQuestRequest);
                return Ok(getDateQuestRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{dateQuestId}", Name = "ToogleDateQuestUser")]
        public async Task<ActionResult> ToogleDateQuestUser([FromRoute] int dateQuestId, [FromBody] ToggleDateQuestRequest getDateQuestRequest)
        {
            try
            {
                await _dateQuestsService.ToggleQuestAsync(dateQuestId, getDateQuestRequest);
                return Ok(getDateQuestRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{dateQuestId}", Name = "DeleteDateQuest")]
        public async Task<ActionResult> DeleteTeams(int dateQuestId)
        {
            try
            {
                await _dateQuestsService.DeleteDateQuestAsync(dateQuestId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
