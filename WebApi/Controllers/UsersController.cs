﻿using BLLLibrary.IService;
using DataLibrary.EmailSender;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.DTO.Request;

namespace WebApi.Controllers
{
    //[Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController(IUsersService usersService, IEmailSenderService emailSenderService) : ControllerBase
    {
        private readonly IUsersService _usersService = usersService;
        private readonly IEmailSenderService _emailSenderService = emailSenderService;

        [HttpGet(Name = "GetAllUsers")]
        public async Task<ActionResult<List<USERS>>> GetAllUsers([FromQuery] GetUsersPaginationRequest getUsersPaginationRequest)
        {
            var users = await _usersService.GetAllUsersAsync(getUsersPaginationRequest);
            return Ok(users);
        }

        [HttpGet("{userId}", Name = "GetUserById")]
        public async Task<ActionResult<USERS>> GetUserById(int userId)
        {
            var user = await _usersService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("login", Name = "GetUserByLoginAndPassword")]
        public async Task<ActionResult<USERS>> GetUserByLoginAndPassword([FromQuery] GetUsersByLoginAndPassword getUsersByLoginAndPassword)
        {
            var user = await _usersService.GetUserByLoginAndPasswordAsync(getUsersByLoginAndPassword);
            await _usersService.SaveChangesAsync();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost(Name = "AddUser")]
        public async Task<ActionResult> AddUser([FromBody] GetUserRequest userRequest)
        {
            try
            {
                await _usersService.AddUserAsync(userRequest);
                await _usersService.SaveChangesAsync();
                return Ok(userRequest);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{userId}", Name = "UpdateUser")]
        public async Task<ActionResult> UpdateUser(int userId, [FromBody] GetUserRequest userRequest)
        {
            var existingUser = await _usersService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound();
            }
            try
            {
                await _usersService.UpdateUserAsync(userRequest, userId);
                await _usersService.SaveChangesAsync();
                return Ok(userRequest);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("column-{userId}", Name = "UpdateColumnUser")]
        public async Task<ActionResult> UpdateColumnUser(int userId, [FromBody] GetUpdateUserRequest getUpdateUserRequest)
        {
            var existingUser = await _usersService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound();
            }
            try
            {
                await _usersService.UpdateColumnUserAsync(getUpdateUserRequest, userId);
                await _usersService.SaveChangesAsync();
                return Ok(getUpdateUserRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{userId}", Name = "DeleteUser")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            var existingUser = await _usersService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound();
            }
            try
            {
                await _usersService.DeleteUserAsync(userId);
                await _usersService.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("withoutGroup", Name = "GetAllUsersWithoutGroupAsync")]
        public async Task<ActionResult<List<USERS>>> GetAllUsersWithoutGroupAsync([FromQuery] GetUsersWithoutGroupPaginationRequest getUsersWithoutGroupPaginationRequest)
        {
            var users = await _usersService.GetAllUsersWithoutGroupAsync(getUsersWithoutGroupPaginationRequest);
            return Ok(users);
        }

        [HttpPost("sendInvitationEmail", Name = "SendInvitationEmail")]
        public async Task<ActionResult> SendInvitationEmail([FromBody] GetEmailSenderRequest getEmailSenderRequest)
        {

            var user = await _usersService.GetUserByEmailAsync(getEmailSenderRequest.To);
            if (user != null) return StatusCode(500, "Account exist with this email");
            bool result = await _emailSenderService.SendInviteMessageAsync(getEmailSenderRequest, new CancellationToken());

            if (result)
            {
                return Ok(getEmailSenderRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }
    }
}