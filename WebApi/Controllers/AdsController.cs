using BLLLibrary.IService;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/ads")]
    [ApiController]
    [Authorize]
    public class AdsController(IAdsService adsService) : ControllerBase
    {
        private readonly IAdsService _adsService = adsService;

        [HttpGet(Name = "GetAd")]
        public async Task<ActionResult<GetAdResponse>> GetAd()
        {
            try
            {
                var ad = await _adsService.GetAdsAsync();
                return Ok(ad);
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


        [HttpGet("all", Name = "GetAds")]
        public async Task<ActionResult<List<GetAdResponse>>> GetAds()
        {
            try
            {
                var ad = await _adsService.GetAllAdsAsync();
                return Ok(ad);
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

        [HttpPost(Name = "PostAd")]
        public async Task<ActionResult> PostClickHistoryAd(PostAdHistoryRequest AdHistory)
        {
            try
            {
                await _adsService.PostClickHistoryAd(AdHistory);
                return Ok();
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
