using BLLLibrary.IService;
using BLLLibrary.Service;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [HttpPost(Name = "AddTeams")]
        public async Task<ActionResult> AddTeams([FromQuery] GetTeamRequest getTeamRequest)
        {
            try
            {
                await _teamsService.AddTeamsAsync(getTeamRequest);
                return Ok(getTeamRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{teamId}", Name = "UpdateTeam")]

        public async Task<ActionResult> UpdateTeam(int teamId, GetTeamRequest getTeamRequest)
        {
            try
            {
                await _teamsService.UpdateTeamAsync(teamId, getTeamRequest);
                await _teamsService.SaveChangesAsync();
                return Ok(getTeamRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize]
        [HttpDelete("{teamId}", Name = "DeleteTeam")]
        public async Task<ActionResult> DeleteTeam(int teamId)
        {
            try
            {
                await _teamsService.DeleteTeamAsync(teamId);
                await _teamsService.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
