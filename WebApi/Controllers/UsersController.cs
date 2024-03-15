using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController(IUsersService usersService) : ControllerBase
    {
        private readonly IUsersService _usersService = usersService;

        [Authorize]
        [HttpGet(Name = "GetAllUsers")]
        public async Task<ActionResult<List<USERS>>> GetAllUsers([FromQuery] GetUsersPaginationRequest getUsersPaginationRequest)
        {
            try
            {
                var users = await _usersService.GetAllUsersAsync(getUsersPaginationRequest);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{userId}", Name = "GetUserById")]
        public async Task<ActionResult<USERS>> GetUserById(int userId)
        {
            try
            {
                var user = await _usersService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("login", Name = "GetUserByLoginAndPassword")]
        public async Task<ActionResult<USERS>> GetUserByLoginAndPassword([FromQuery] GetUsersByLoginAndPasswordRequest getUsersByLoginAndPassword)
        {
            try
            {
                var user = await _usersService.GetUserByLoginAndPasswordAsync(getUsersByLoginAndPassword);
                await _usersService.SaveChangesAsync();
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost(Name = "AddUser")]
        public async Task<ActionResult> AddUser([FromBody] GetUserRequest userRequest)
        {
            try
            {
                await _usersService.AddUserAsync(userRequest);
                await _usersService.SaveChangesAsync();
                return Ok(userRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("{userId}", Name = "UpdateUser")]
        public async Task<ActionResult> UpdateUser(int userId, [FromBody] GetUserRequest userRequest)
        {
            try
            {
                var existingUser = await _usersService.GetUserByIdAsync(userId);
                if (existingUser == null)
                {
                    return NotFound();
                }
                await _usersService.UpdateUserAsync(userRequest, userId);
                await _usersService.SaveChangesAsync();
                return Ok(userRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPut("column-{userId}", Name = "UpdateColumnUser")]
        public async Task<ActionResult> UpdateColumnUser(int userId, [FromBody] GetUpdateUserRequest getUpdateUserRequest)
        {
            try
            {
                var existingUser = await _usersService.GetUserByIdAsync(userId);
                if (existingUser == null)
                {
                    return NotFound();
                }
                await _usersService.UpdateColumnUserAsync(getUpdateUserRequest, userId);
                await _usersService.SaveChangesAsync();
                return Ok(getUpdateUserRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{userId}", Name = "DeleteUser")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("withoutGroup", Name = "GetAllUsersWithoutGroup")]
        public async Task<ActionResult<List<USERS>>> GetAllUsersWithoutGroup([FromQuery] GetUsersWithoutGroupPaginationRequest getUsersWithoutGroupPaginationRequest)
        {
            try
            {
                var users = await _usersService.GetAllUsersWithoutGroupAsync(getUsersWithoutGroupPaginationRequest);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPut(Name = "changePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] GetUsersByLoginAndPasswordRequest getUsersByLoginAndPassword)
        {
            try
            {
                await _usersService.ChangePassword(getUsersByLoginAndPassword);
                return Ok(getUsersByLoginAndPassword);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
