using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/ads")]
    [ApiController]
    public class AdsController(IAdsService adsService) : ControllerBase
    {
        private readonly IAdsService _adsService = adsService;

        [Authorize]
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
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
