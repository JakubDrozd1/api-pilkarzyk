using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.DTO.Request;

namespace WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController(IUsersService usersService) : ControllerBase
    {
        private readonly IUsersService _usersService = usersService;

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
    }
}
