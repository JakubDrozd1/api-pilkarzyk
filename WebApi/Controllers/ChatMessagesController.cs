using BLLLibrary.IService;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataLibrary.Model.DTO.Request.TableRequest;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/chat-messages")]
    [ApiController]
    public class ChatMessagesController(IChatMessagesService chatMessagesService) : ControllerBase
    {
        private readonly IChatMessagesService _chatMessagesService = chatMessagesService;

        [HttpGet(Name = "GetAllChatMessagesFromMeeting")]
        public async Task<ActionResult<List<GetChatMessagesResponse>>> GetAllChatMessagesFromMeeting([FromQuery] GetChatMessagesPaginationRequest getChatMessagesPaginationRequest)
        {
            try
            {
                var chatMessages = await _chatMessagesService.GetAllChatMessagesFromMeeting(getChatMessagesPaginationRequest);
                return Ok(chatMessages);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost(Name = "AddMessageToChat")]
        public async Task<ActionResult> AddMessageToChat([FromBody] GetChatMessageRequest getChatMessageRequest)
        {
            try
            {
                await _chatMessagesService.AddMessageToChat(getChatMessageRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
