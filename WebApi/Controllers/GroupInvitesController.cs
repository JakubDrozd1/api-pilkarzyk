using BLLLibrary.IService;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/group-invites")]
    [ApiController]
    public class GroupInvitesController(IGroupInviteService groupInviteService) : ControllerBase
    {
        private readonly IGroupInviteService _groupInviteService = groupInviteService;

        [HttpGet("{userId}", Name = "GetGroupInviteByIdUserAsync")]
        public async Task<ActionResult<List<GetGroupInviteResponse>>> GetGroupInviteByIdUserAsync(int userId)
        {
            try
            {
                var group = await _groupInviteService.GetGroupInviteByIdUserAsync(userId);
                return Ok(group);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost(Name = "AddGroupInviteAsync")]
        public async Task<ActionResult> AddGroupInviteAsync([FromBody] GetGroupInviteRequest getGroupInviteRequest)
        {
            try
            {
                await _groupInviteService.AddGroupInviteAsync(getGroupInviteRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{groupInvitedId}", Name = "DeleteGroupInviteAsync")]
        public async Task<ActionResult> DeleteGroupInviteAsync(int groupInvitedId)
        {
            try
            {
                await _groupInviteService.DeleteGroupInviteAsync(groupInvitedId);
                await _groupInviteService.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
