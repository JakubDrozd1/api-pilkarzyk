using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/reset-password")]
    [AllowAnonymous]
    [ApiController]
    public class ResetPasswordsController(IResetPasswordService resetPasswordService, IEmailSenderService emailSenderService) : ControllerBase
    {
        private readonly IEmailSenderService _emailSenderService = emailSenderService;
        private readonly IResetPasswordService _resetPasswordService = resetPasswordService;

        [HttpPost(Name = "SendRecoveryPasswordEmail")]
        public async Task<ActionResult> SendRecoveryPasswordEmail([FromQuery] string email)
        {
            try
            {
                var user = await _resetPasswordService.AddResetPasswordAsync(email);
                bool result = await _emailSenderService.SendRecoveryPasswordMessageAsync(email, user?.ID_RESET_PASSWORD ?? throw new Exception(), new CancellationToken());
                if (result)
                {
                    return Ok(email);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
                }
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
