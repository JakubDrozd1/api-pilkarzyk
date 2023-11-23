using BLLLibrary.IService;
using BLLLibrary.Model.DTO.Response;
using DataLibrary.Model.DTO.Request;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/groups-users")]
    [ApiController]
    public class GroupsUsersController(IGroupsUsersService groupsUsersService) : ControllerBase
    {
        private readonly IGroupsUsersService _groupsUsersService = groupsUsersService;

        [HttpPost("add")]
        public async Task<IActionResult> AddUserToGroupAsync(int userId, int groupId)
        {
            await _groupsUsersService.AddUserToGroupAsync(userId, groupId);
            return Ok();
        }

        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteAllGroupsFromUser(int userId)
        {
            await _groupsUsersService.DeleteAllGroupsFromUser(userId);
            return Ok();
        }

        [HttpDelete("group/{groupId}")]
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

        [HttpGet("all")]
        public async Task<ActionResult<List<GetGroupsUsersResponse>>> GetAllGroupsFromUserAsync([FromQuery] GetUsersGroupsPaginationRequest getUsersGroupsPaginationRequest)
        {
            var result = await _groupsUsersService.GetListGroupsUserAsync(getUsersGroupsPaginationRequest);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<GetGroupsUsersResponse?>> GetUserWithGroup(int groupId, int userId)
        {
            var result = await _groupsUsersService.GetUserWithGroup(groupId, userId);
            return Ok(result);
        }

        [HttpPut("update-group/{groupId}")]
        public async Task<IActionResult> UpdateGroupWithUsersAsync(int[] usersId, int groupId)
        {
            await _groupsUsersService.UpdateGroupWithUsersAsync(usersId, groupId);
            return Ok();
        }

        [HttpPut("update-user/{userId}")]
        public async Task<IActionResult> UpdateUserWithGroupsAsync(int[] groupsId, int userId)
        {
            await _groupsUsersService.UpdateUserWithGroupsAsync(groupsId, userId);
            return Ok();
        }
    }
}
