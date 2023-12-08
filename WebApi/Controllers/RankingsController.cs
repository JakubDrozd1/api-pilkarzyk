using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.DTO.Request;

namespace WebApi.Controllers
{
    //[Authorize]
    [Route("api/rankings")]
    [ApiController]
    public class RankingsController(IRankingsService rankingsService) : ControllerBase
    {
        private readonly IRankingsService _rankingsService = rankingsService;

        [HttpGet(Name = "GetAllRankings")]
        public async Task<ActionResult<List<GetRankingsUsersGroupsResponse>>> GetAllRankings([FromQuery] GetRankingsUsersGroupsPaginationRequest getRankingsUsersGroupsPaginationRequest)
        {
            var rankings = await _rankingsService.GetAllRankingsAsync(getRankingsUsersGroupsPaginationRequest);
            return Ok(rankings);
        }

        [HttpGet("{rankingId}", Name = "GetRankingById")]
        public async Task<ActionResult<RANKINGS>> GetRankingById(int rankingId)
        {
            var ranking = await _rankingsService.GetRankingByIdAsync(rankingId);
            if (ranking == null)
            {
                return NotFound();
            }
            return Ok(ranking);
        }

        [HttpPost(Name = "AddRanking")]
        public async Task<ActionResult> AddRanking([FromBody] GetRankingRequest rankingRequest)
        {
            try
            {
                await _rankingsService.AddRankingAsync(rankingRequest);
                await _rankingsService.SaveChangesAsync();
                return Ok(rankingRequest);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{rankingId}", Name = "UpdateRanking")]
        public async Task<ActionResult> UpdateRanking(int rankingId, [FromBody] GetRankingRequest rankingRequest)
        {
            var existingRanking = await _rankingsService.GetRankingByIdAsync(rankingId);
            if (existingRanking == null)
            {
                return NotFound();
            }
            try
            {
                await _rankingsService.UpdateRankingAsync(rankingRequest, rankingId);
                await _rankingsService.SaveChangesAsync();
                return Ok(_rankingsService);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{rankingId}", Name = "DeleteRanking")]
        public async Task<ActionResult> DeleteRanking(int rankingId)
        {
            var existingRanking = await _rankingsService.GetRankingByIdAsync(rankingId);
            if (existingRanking == null)
            {
                return NotFound();
            }
            try
            {
                await _rankingsService.DeleteRankingAsync(rankingId);
                await _rankingsService.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
