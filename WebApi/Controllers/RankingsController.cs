using BLLLibrary.IService;
using DataLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.DTO.Request;

namespace WebApi.Controllers
{
    [Route("api/rankings")]
    [ApiController]
    public class RankingsController(IRankingsService rankingsService) : ControllerBase
    {
        private readonly IRankingsService _rankingsService = rankingsService;

        [HttpGet]
        public async Task<ActionResult<List<Ranking>>> GetAllRankings()
        {
            var rankings = await _rankingsService.GetAllRankingsAsync();
            return Ok(rankings);
        }

        [HttpGet("{rankingId}")]
        public async Task<ActionResult<Ranking>> GetRankingById(int rankingId)
        {
            var ranking = await _rankingsService.GetRankingByIdAsync(rankingId);
            if (ranking == null)
            {
                return NotFound();
            }
            return Ok(ranking);
        }

        [HttpPost]
        public async Task<ActionResult> AddRanking([FromBody] RankingRequest rankingRequest)
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

        [HttpPut("{rankingId}")]
        public async Task<ActionResult> UpdateRanking(int rankingId, [FromBody] RankingRequest rankingRequest)
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

        [HttpDelete("{rankingId}")]
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
