using BLLLibrary.IService;
using DataLibrary.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/teams")]
    [ApiController]
    [Authorize]
    public class TeamsController(ITeamsService teamsService) : ControllerBase
    {
        private readonly ITeamsService _teamsService = teamsService;

        [HttpGet("{meetingId}", Name = "GetAllTeamsFromMeeting")]
        public async Task<ActionResult<List<TEAMS>>> GetAllTeamsFromMeeting(int meetingId)
        {
            try
            {
                var teams = await _teamsService.GetTeamByMeetingIdAsync(meetingId);
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
