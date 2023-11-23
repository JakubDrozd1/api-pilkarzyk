using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.DTO.Request;

namespace WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController(IUsersService usersService) : ControllerBase
    {
        private readonly IUsersService _usersService = usersService;

        [HttpGet]
        public async Task<ActionResult<List<USERS>>> GetAllUsers([FromQuery] GetUsersPaginationRequest getUsersPaginationRequest)
        {
            var users = await _usersService.GetAllUsersAsync(getUsersPaginationRequest);
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<USERS>> GetUserById(int userId)
        {
            var user = await _usersService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
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

        [HttpPut("{userId}")]
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

        [HttpDelete("{userId}")]
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
