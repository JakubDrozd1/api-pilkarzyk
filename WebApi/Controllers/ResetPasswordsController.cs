using BLLLibrary.IService;
using DataLibrary.Helper.Email;
using DataLibrary.Model.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/reset-password")]
    [AllowAnonymous]
    [ApiController]
    public class ResetPasswordsController(IResetPasswordService resetPasswordService) : ControllerBase
    {
        private readonly IResetPasswordService _resetPasswordService = resetPasswordService;

        [HttpPost(Name = "SendRecoveryPasswordEmail")]
        public async Task<ActionResult> SendRecoveryPasswordEmail([FromQuery] string email)
        {
            try
            {
                await _resetPasswordService.AddResetPasswordAsync(email);
                return Ok(email);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("{resetPasswordId}", Name = "GetResetPasswordById")]
        public async Task<ActionResult<GetResetPasswordResponse>> GetResetPasswordById(int resetPasswordId)
        {
            try
            {
                var resetPassword = await _resetPasswordService.GetResetPasswordByIdAsync(resetPasswordId);
                if (resetPassword == null)
                {
                    return NotFound();
                }
                return Ok(resetPassword);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
