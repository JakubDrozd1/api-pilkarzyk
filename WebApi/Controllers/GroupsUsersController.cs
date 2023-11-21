using BLLLibrary.IService;
using BLLLibrary.Model.DTO.Response;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/groups-users")]
    [ApiController]
    public class GroupsUsersController(IGroupsUsersService groupsUsersService) : ControllerBase
    {
        private readonly IGroupsUsersService _groupsUsersService = groupsUsersService;

        [HttpPost("AddUserToGroup")]
        public async Task<IActionResult> AddUserToGroupAsync(int userId, int groupId)
        {
            await _groupsUsersService.AddUserToGroupAsync(userId, groupId);
            return Ok();
        }

        [HttpDelete("DeleteAllGroupsFromUser")]
        public async Task<IActionResult> DeleteAllGroupsFromUser(int userId)
        {
            await _groupsUsersService.DeleteAllGroupsFromUser(userId);
            return Ok();
        }

        [HttpDelete("DeleteAllUsersFromGroup")]
        public async Task<IActionResult> DeleteAllUsersFromGroupAsync(int groupId)
        {
            await _groupsUsersService.DeleteAllUsersFromGroupAsync(groupId);
            return Ok();
        }

        [HttpDelete("DeleteUsersFromGroup")]
        public async Task<IActionResult> DeleteUsersFromGroupAsync(int[] usersId, int groupId)
        {
            await _groupsUsersService.DeleteUsersFromGroupAsync(usersId, groupId);
            return Ok();
        }

        [HttpGet("GetAllGroupsFromUser")]
        public async Task<ActionResult<List<GetGroupsWithUsersResponse>>> GetAllGroupsFromUserAsync(int userId)
        {
            var result = await _groupsUsersService.GetAllGroupsFromUserAsync(userId);
            return Ok(result);
        }

        [HttpGet("GetAllUsersFromGroup")]
        public async Task<ActionResult<List<GetGroupsWithUsersResponse>>> GetAllUsersFromGroupAsync(int groupId)
        {
            var result = await _groupsUsersService.GetAllUsersFromGroupAsync(groupId);
            return Ok(result);
        }

        [HttpGet("GetUserWithGroup")]
        public async Task<ActionResult<GetGroupsWithUsersResponse?>> GetUserWithGroup(int groupId, int userId)
        {
            var result = await _groupsUsersService.GetUserWithGroup(groupId, userId);
            return Ok(result);
        }

        [HttpPut("UpdateGroupWithUsers")]
        public async Task<IActionResult> UpdateGroupWithUsersAsync(int[] usersId, int groupId)
        {
            await _groupsUsersService.UpdateGroupWithUsersAsync(usersId, groupId);
            return Ok();
        }

        [HttpPut("UpdateUserWithGroups")]
        public async Task<IActionResult> UpdateUserWithGroupsAsync(int[] groupsId, int userId)
        {
            await _groupsUsersService.UpdateUserWithGroupsAsync(groupsId, userId);
            return Ok();
        }
    }
}
