using BLLLibrary.IService;
using DataLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.DTO.Request;

namespace WebApi.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupsController(IGroupsService groupsService) : ControllerBase
    {
        private readonly IGroupsService _groupsService = groupsService;

        [HttpGet]
        public async Task<ActionResult<List<Groupe>>> GetAllGroups()
        {
            var group = await _groupsService.GetAllGroupsAsync();
            return Ok(group);
        }

        [HttpGet("{groupId}")]
        public async Task<ActionResult<Groupe>> GetGroupById(int groupId)
        {
            var group = await _groupsService.GetGroupByIdAsync(groupId);
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group);
        }

        [HttpPost]
        public async Task<ActionResult> AddGroup([FromBody] GroupRequest groupRequest)
        {
            try
            {
                await _groupsService.AddGroupAsync(groupRequest);
                await _groupsService.SaveChangesAsync();
                return Ok(groupRequest);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }


        }

        [HttpPut("{groupId}")]
        public async Task<ActionResult> UpdateGroup(int groupId, [FromBody] GroupRequest groupRequest)
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

        [HttpDelete("{groupId}")]
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
