using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/notification")]
    [Authorize]
    [ApiController]
    public class NotificationsController(INotificationService notificationService) : ControllerBase
    {
        private readonly INotificationService _notificationServic = notificationService;


        [HttpPost(Name = "AddNotificationToUser")]
        public async Task<ActionResult> AddNotificationToUser([FromBody] GetNotificationRequest getNotificationRequest)
        {
            try
            {
                await _notificationServic.AddNotificationToUserAsync(getNotificationRequest);
                await _notificationServic.SaveChangesAsync();
                return Ok(getNotificationRequest);
            }
            catch (Exception ex)
            {
                if(ex.Source == "FirebirdSql.Data.FirebirdClient")
                {
                    return StatusCode(500);
                }
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("{userId}", Name = "GetAllNotificationFromUser")]
        public async Task<ActionResult<NOTIFICATION>> GetAllNotificationFromUser(int userId)
        {
            try
            {
                var notifications = await _notificationServic.GetAllNotificationFromUser(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                if(ex.Source == "FirebirdSql.Data.FirebirdClient")
                {
                    return StatusCode(500);
                }
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{userId}/message", Name = "GetAllNotificationMessageFromUser")]
        public async Task<ActionResult<List<NOTIFICATION_MESSAGES>>> GetAllNotificationMessageFromUser(int userId)
        {
            try
            {
                var notificationsMessage = await _notificationServic.GetAllNotificationMessageFromUser(userId);
                return Ok(notificationsMessage);
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

        [HttpDelete("{userId}", Name = "DeletaAllNotificationFromUser")]
        public async Task<ActionResult> DeletaAllNotificationFromUser(int userId)
        {
            try
            {
                await _notificationServic.DeletaAllNotificationFromUser(userId);
                await _notificationServic.SaveChangesAsync();
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

        [HttpPut("column-{userId}", Name = "UpdateColumnNotification")]
        public async Task<ActionResult> UpdateColumnNotification(int userId, [FromBody] GetUpdateNotificationRequest getUpdateNotificationRequest)
        {
            try
            {
                await _notificationServic.UpdateColumnNotificationAsync(getUpdateNotificationRequest, userId);
                await _notificationServic.SaveChangesAsync();
                return Ok(getUpdateNotificationRequest);
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
