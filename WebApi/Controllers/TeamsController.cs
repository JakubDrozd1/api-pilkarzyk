using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.TableRequest;
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
                if (ex.Source == "FirebirdSql.Data.FirebirdClient")
                {
                    return StatusCode(500);
                }
                return BadRequest(new { message = ex.Message });
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
                if (ex.Source == "FirebirdSql.Data.FirebirdClient")
                {
                    return StatusCode(500);
                }
                return BadRequest(new { message = ex.Message });
            }
        }

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
                if (ex.Source == "FirebirdSql.Data.FirebirdClient")
                {
                    return StatusCode(500);
                }
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("bulkUpdate", Name = "BulkUpdateTeamsMeeting")]
        public async Task<ActionResult> BulkUpdateTeamsMeeting(GetUpdateBulkTeamRequest getUpdateBulkTeamRequest)
        {
            try
            {
                await _teamsService.BulkUpdateTeamsMeeting(getUpdateBulkTeamRequest);
                return Ok(getUpdateBulkTeamRequest);
            }
            catch (Exception ex)
            {
                if (ex.Source == "FirebirdSql.Data.FirebirdClient")
                {
                    return StatusCode(500);
                }
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
