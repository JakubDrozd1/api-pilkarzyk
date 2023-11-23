﻿using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.DTO.Request;

namespace WebApi.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessagesController(IMessagesService messagesService) : ControllerBase
    {
        private readonly IMessagesService _messagesService = messagesService;

        [HttpGet]
        public async Task<ActionResult<List<MESSAGES>>> GetAllMessages([FromQuery] GetMessagesUsersPaginationRequest getMessagesUsersPaginationRequest)
        {
            var messages = await _messagesService.GetAllMessagesAsync(getMessagesUsersPaginationRequest);
            return Ok(messages);
        }
        
        [HttpGet("{messageId}")]
        public async Task<ActionResult<MESSAGES>> GetMessageById(int messageId)
        {
            var message = await _messagesService.GetMessageByIdAsync(messageId);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        [HttpPost]
        public async Task<ActionResult> AddMessage([FromBody] GetMessageRequest messageRequest)
        {
            try
            {
                await _messagesService.AddMessageAsync(messageRequest);
                await _messagesService.SaveChangesAsync();
                return Ok(messageRequest);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{messageId}")]
        public async Task<ActionResult> UpdateMessage(int messageId, [FromBody] GetMessageRequest messageRequest)
        {
            var existingMessage = await _messagesService.GetMessageByIdAsync(messageId);
            if (existingMessage == null)
            {
                return NotFound();
            }
            try
            {
                await _messagesService.UpdateMessageAsync(messageRequest, messageId);
                await _messagesService.SaveChangesAsync();
                return Ok(messageRequest);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{messageId}")]
        public async Task<ActionResult> DeleteMessage(int messageId)
        {
            var existingMessage = await _messagesService.GetMessageByIdAsync(messageId);

            if (existingMessage == null)
            {
                return NotFound();
            }
            try
            {
                await _messagesService.DeleteMessageAsync(messageId);
                await _messagesService.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
