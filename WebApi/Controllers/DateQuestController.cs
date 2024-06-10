using BLLLibrary.IService;
using BLLLibrary.IService;
using BLLLibrary.Service;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<ActionResult> AddTeams([FromQuery] GetDateQuestRequest getDateQuestRequest)
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
