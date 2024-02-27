using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/messages")]
    [ApiController]
    public class MessagesController(IMessagesService messagesService) : ControllerBase
    {
        private readonly IMessagesService _messagesService = messagesService;

        [HttpGet(Name = "GetAllMessages")]
        public async Task<ActionResult<List<GetMessagesUsersMeetingsResponse>>> GetAllMessages([FromQuery] GetMessagesUsersPaginationRequest getMessagesUsersPaginationRequest)
        {
            try
            {
                var messages = await _messagesService.GetAllMessagesAsync(getMessagesUsersPaginationRequest);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{messageId}", Name = "GetMessageById")]
        public async Task<ActionResult<MESSAGES>> GetMessageById(int messageId)
        {
            try
            {
                var message = await _messagesService.GetMessageByIdAsync(messageId);
                if (message == null)
                {
                    return NotFound();
                }
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost(Name = "AddMessage")]
        public async Task<ActionResult> AddMessage([FromBody] GetMessageRequest messageRequest)
        {
            try
            {
                await _messagesService.AddMessageAsync(messageRequest);
                await _messagesService.SaveChangesAsync();
                return Ok(messageRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{messageId}", Name = "UpdateMessage")]
        public async Task<ActionResult> UpdateMessage(int messageId, [FromBody] GetMessageRequest messageRequest)
        {
            try
            {
                var existingMessage = await _messagesService.GetMessageByIdAsync(messageId);
                if (existingMessage == null)
                {
                    return NotFound();
                }
                await _messagesService.UpdateMessageAsync(messageRequest, messageId);
                await _messagesService.SaveChangesAsync();
                return Ok(messageRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("answer", Name = "UpdateAnswerMessage")]
        public async Task<ActionResult> UpdateAnswerMessage([FromBody] GetMessageRequest messageRequest)
        {
            try
            {
                await _messagesService.UpdateAnswerMessageAsync(messageRequest);
                await _messagesService.SaveChangesAsync();
                return Ok(messageRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{messageId}", Name = "DeleteMessage")]
        public async Task<ActionResult> DeleteMessage(int messageId)
        {
            try
            {
                var existingMessage = await _messagesService.GetMessageByIdAsync(messageId);
                if (existingMessage == null)
                {
                    return NotFound();
                }
                await _messagesService.DeleteMessageAsync(messageId);
                await _messagesService.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
