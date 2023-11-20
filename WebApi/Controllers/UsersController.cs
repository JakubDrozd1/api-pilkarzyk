using BLLLibrary.IService;
using DataLibrary.Entities;
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
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _usersService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUserById(int userId)
        {
            var user = await _usersService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        //TODO
        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] UserRequest userRequest)
        {
            await _usersService.AddUserAsync(userRequest);
            await _usersService.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserById), userRequest);
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult> UpdateUser(int userId, [FromBody] UserRequest userRequest)
        {
            var existingUser = await _usersService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound();
            }
            await _usersService.UpdateUserAsync(userRequest);
            await _usersService.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            var existingUser = await _usersService.GetUserByIdAsync(userId);

            if (existingUser == null)
            {
                return NotFound();
            }

            await _usersService.DeleteUserAsync(userId);
            await _usersService.SaveChangesAsync();

            return NoContent();
        }
    }
}
