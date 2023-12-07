using BLLLibrary;
using DataLibrary.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace WebApi.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController(ITokenService tokenService) : ControllerBase
    {
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost(Name = "GenerateJwtTokenAsync")]
        public async Task<IActionResult> GenerateJwtTokenAsync([FromBody] GetTokenRequest getTokenRequest)
        {
            try
            {
                var tokenResponse = await _tokenService.GenerateJwtTokenAsync(getTokenRequest);
                return Ok(tokenResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("generate", Name = "GenerateToken")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetTokenResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Generowanie tokenu JWT", Description = "Generuje token JWT na podstawie danych autoryzacyjnych.", OperationId = "GenerateToken")]
        public async Task<IActionResult> GenerateToken([FromForm] GetTokenRequest tokenRequest)
        {
            try
            {
                tokenRequest.Client_secret = "@%7fMQSMMmhc5x40M8S4Y%A%h7l7!5Zcfkm!uXKL8nzvYO%ITc4P!hm14ENP08GD*Nh8XWumaL*yEur8";
                tokenRequest.Client_id = "api.pilkarzyk";
                var tokenResponse = await _tokenService.GenerateJwtTokenAsync(tokenRequest);
                await _tokenService.SaveChangesAsync();
                return Ok(tokenResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
