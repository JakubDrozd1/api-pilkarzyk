﻿using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
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
        [AllowAnonymous]
        [HttpGet("{groupInviteId}", Name = "GetGroupInviteById")]
        public async Task<ActionResult<GROUP_INVITE>> GetGroupInviteById(int groupInviteId)
        {
            try
            {
                var group = await _groupInviteService.GetGroupInviteByIdAsync(groupInviteId);
                return Ok(group);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost(Name = "AddGroupInvite")]
        public async Task<ActionResult> AddGroupInvite([FromBody] GetGroupInviteWithEmailOrPhoneRequest getGroupInviteRequest)
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
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("multiple", Name = "AddMultipleGroupInvite")]
        public async Task<ActionResult> AddMultipleGroupInvite([FromBody] GetMultipleGroupInviteRequest getMultipleGroupInviteRequest)
        {
            try
            {
                await _groupInviteService.AddMultipleGroupInviteAsync(getMultipleGroupInviteRequest);
                return Ok(getMultipleGroupInviteRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
