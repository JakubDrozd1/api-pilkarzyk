using System.ComponentModel.DataAnnotations;
using BLLLibrary.IService;
using BLLLibrary.Model.DTO.Response;
using DataLibrary.Model.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/groups-users")]
    [ApiController]
    [Authorize]
    public class GroupsUsersController(IGroupsUsersService groupsUsersService) : ControllerBase
    {
        private readonly IGroupsUsersService _groupsUsersService = groupsUsersService;

        [HttpPost("add", Name = "AddUserToGroupAsync")]
        public async Task<IActionResult> AddUserToGroupAsync([FromQuery, Required] int userId, [FromQuery, Required] int groupId)
        {
            await _groupsUsersService.AddUserToGroupAsync(userId, groupId);
            return Ok();
        }

        [HttpDelete("user/{userId}", Name = "DeleteAllGroupsFromUser")]
        public async Task<IActionResult> DeleteAllGroupsFromUser(int userId)
        {
            await _groupsUsersService.DeleteAllGroupsFromUser(userId);
            return Ok();
        }

        [HttpDelete("group/{groupId}", Name = "DeleteAllUsersFromGroupAsync")]
        public async Task<IActionResult> DeleteAllUsersFromGroupAsync(int groupId)
        {
            await _groupsUsersService.DeleteAllUsersFromGroupAsync(groupId);
            return Ok();
        }

        [HttpDelete("users/{groupId}", Name = "DeleteUsersFromGroupAsync")]
        public async Task<IActionResult> DeleteUsersFromGroupAsync(int[] usersId, int groupId)
        {
            await _groupsUsersService.DeleteUsersFromGroupAsync(usersId, groupId);
            return Ok();
        }

        [HttpGet("all", Name = "GetAllGroupsFromUserAsync")]
        public async Task<ActionResult<List<GetGroupsUsersResponse>>> GetAllGroupsFromUserAsync([FromQuery] GetUsersGroupsPaginationRequest getUsersGroupsPaginationRequest)
        {
            var result = await _groupsUsersService.GetListGroupsUserAsync(getUsersGroupsPaginationRequest);
            return Ok(result);
        }

        [HttpGet(Name = "GetUserWithGroup")]
        public async Task<ActionResult<GetGroupsUsersResponse?>> GetUserWithGroup([FromQuery, Required] int groupId, [FromQuery, Required] int userId)
        {
            var result = await _groupsUsersService.GetUserWithGroup(groupId, userId);
            return Ok(result);
        }

        [HttpPut("update-group/{groupId}", Name = "UpdateGroupWithUsersAsync")]
        public async Task<IActionResult> UpdateGroupWithUsersAsync(int[] usersId, int groupId)
        {
            await _groupsUsersService.UpdateGroupWithUsersAsync(usersId, groupId);
            return Ok();
        }

        [HttpPut("update-user/{userId}", Name = "UpdateUserWithGroupsAsync")]
        public async Task<IActionResult> UpdateUserWithGroupsAsync(int[] groupsId, int userId)
        {
            await _groupsUsersService.UpdateUserWithGroupsAsync(groupsId, userId);
            return Ok();
        }
    }
}
