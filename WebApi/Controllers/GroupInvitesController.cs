using BLLLibrary.IService;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
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

        [HttpGet(Name = "GetGroupInviteByIdUser")]
        public async Task<ActionResult<List<GetGroupInviteResponse>>> GetGroupInviteByIdUser([FromQuery] GetGroupInvitePaginationRequest getGroupInvitePaginationRequest)
        {
            try
            {
                var group = await _groupInviteService.GetGroupInviteByIdUserAsync(getGroupInvitePaginationRequest);
                return Ok(group);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost(Name = "AddGroupInvite")]
        public async Task<ActionResult> AddGroupInvite([FromBody] GetGroupInviteRequest getGroupInviteRequest)
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

        [HttpDelete("{groupInvitedId}", Name = "DeleteGroupInvite")]
        public async Task<ActionResult> DeleteGroupInvite(int groupInvitedId)
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
