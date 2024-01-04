using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/groups")]
    [ApiController]
    public class GroupsController(IGroupsService groupsService) : ControllerBase
    {
        private readonly IGroupsService _groupsService = groupsService;
        [HttpGet(Name = "GetAllGroups")]
        public async Task<ActionResult<List<GROUPS>>> GetAllGroups([FromQuery] GetGroupsPaginationRequest getGroupsPaginationRequest)
        {
            var group = await _groupsService.GetAllGroupsAsync(getGroupsPaginationRequest);
            return Ok(group);
        }

        [HttpGet("{groupId}", Name = "GetGroupById")]
        public async Task<ActionResult<GROUPS>> GetGroupById(int groupId)
        {
            var group = await _groupsService.GetGroupByIdAsync(groupId);
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group);
        }

        [HttpPost(Name = "AddGroup")]
        public async Task<ActionResult> AddGroup([FromBody] GetGroupRequest groupRequest)
        {
            try
            {
                await _groupsService.AddGroupAsync(groupRequest);
                await _groupsService.SaveChangesAsync();
                return Ok(await _groupsService.GetGroupByNameAsync(groupRequest.NAME));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{groupId}", Name = "UpdateGroup")]
        public async Task<ActionResult> UpdateGroup(int groupId, [FromBody] GetGroupRequest groupRequest)
        {
            var existingGroup = await _groupsService.GetGroupByIdAsync(groupId);
            if (existingGroup == null)
            {
                return NotFound();
            }
            try
            {
                await _groupsService.UpdateGroupAsync(groupRequest, groupId);
                await _groupsService.SaveChangesAsync();
                return Ok(groupRequest);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{groupId}", Name = "DeleteGroup")]
        public async Task<ActionResult> DeleteGroup(int groupId)
        {
            var existingGroup = await _groupsService.GetGroupByIdAsync(groupId);

            if (existingGroup == null)
            {
                return NotFound();
            }
            try
            {
                await _groupsService.DeleteGroupAsync(groupId);
                await _groupsService.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
