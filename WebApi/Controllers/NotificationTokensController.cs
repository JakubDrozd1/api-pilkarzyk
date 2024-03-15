using BLLLibrary.IService;
using BLLLibrary.Service;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/notification-token")]
    [ApiController]
    public class NotificationTokensController(INotificationTokenService notificationTokenService) : ControllerBase
    {
        private readonly INotificationTokenService _notificationTokenService = notificationTokenService;

        [HttpPost(Name = "AddNotificationToken")]
        public async Task<ActionResult> AddNotificationToken([FromBody] GetNotificationTokenRequest getNotificationTokenRequest)
        {
            try
            {
                await _notificationTokenService.AddTokenToUserAsync(getNotificationTokenRequest);
                await _notificationTokenService.SaveChangesAsync();
                return Ok(getNotificationTokenRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("{userId}", Name = "GetNotificationTokensFromUser")]
        public async Task<ActionResult<List<GetMeetingGroupsResponse>>> GetNotificationTokensFromUser(int userId)
        {
            try
            {
                var notificationTokens = await _notificationTokenService.GetAllTokensFromUser(userId);
                return Ok(notificationTokens);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{userId}", Name = "DeleteNotificationToken")]
        public async Task<ActionResult> DeleteNotificationToken([FromQuery] string token, int userId)
        {
            try
            {
                await _notificationTokenService.DeleteNotificationTokenAsync(token, userId);
                await _notificationTokenService.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
