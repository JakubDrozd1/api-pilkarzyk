using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace WebApi.Controllers
{
    //[Authorize]
    [Route("api/groupsUsersLink")]
    [ApiController]
    public class GroupsUsersLinkController(IGroupsUsersLinkService groupsUsersLinkService) : ControllerBase
    {
        private readonly IGroupsUsersLinkService _groupsUsersLinkService = groupsUsersLinkService;
        [Authorize]
        [HttpGet(Name = "GetLink")]
        public async Task<ActionResult<string>> GetLinkAsync([FromQuery] GetGroupsUsersLinkRequest request)
        {
            try
            {
                var link = await _groupsUsersLinkService.GetLinkAsync(request.groupId, request.userId);
                return Ok(link);
            }
            catch (Exception ex)
            {
                if (ex.Source == "FirebirdSql.Data.FirebirdClient")
                {
                    return StatusCode(500);
                }
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost(Name = "PostLink")]
        public async Task<ActionResult<string>> PostLinkAsync([FromQuery] GetGroupsUsersLinkRequest request)
        {
            try
            {
                var link = await _groupsUsersLinkService.PostLinkAsync(request.groupId, request.userId);
                return Ok(link);
            }
            catch (Exception ex)
            {
                if (ex.Source == "FirebirdSql.Data.FirebirdClient")
                {
                    return StatusCode(500);
                }
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete(Name = "DeleteLink")]
        public async Task<ActionResult> DeleteLinkAsync([FromQuery] GetGroupsUsersLinkRequest request)
        {
            try
            {
                await _groupsUsersLinkService.DeleteLinkAsync(request.groupId, request.userId);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Source == "FirebirdSql.Data.FirebirdClient")
                {
                    return StatusCode(500);
                }
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{code}", Name = "GetLinkByCode/{code}")]
        public async Task<ActionResult<GROUPS_USERS_LINK>> GetLinkByCodeAsync(string code)
        {
            try
            {
                var result = await _groupsUsersLinkService.GetLinkByCodeAsync(code);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Source == "FirebirdSql.Data.FirebirdClient")
                {
                    return StatusCode(500);
                }
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
