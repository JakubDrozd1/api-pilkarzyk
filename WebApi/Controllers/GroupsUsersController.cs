﻿using System.ComponentModel.DataAnnotations;
using BLLLibrary.IService;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/groups-users")]
    [ApiController]
    public class GroupsUsersController(IGroupsUsersService groupsUsersService) : ControllerBase
    {
        private readonly IGroupsUsersService _groupsUsersService = groupsUsersService;

        [HttpPost("add", Name = "AddUserToGroup")]
        public async Task<IActionResult> AddUserToGroup([FromQuery, Required] GetUserGroupRequest getUserGroupRequest)
        {
            try
            {
                await _groupsUsersService.AddUserToGroupAsync(getUserGroupRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("user/{userId}", Name = "DeleteAllGroupsFromUser")]
        public async Task<IActionResult> DeleteAllGroupsFromUser(int userId)
        {
            try
            {
                await _groupsUsersService.DeleteAllGroupsFromUser(userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("group/{groupId}", Name = "DeleteAllUsersFromGroupAsync")]
        public async Task<IActionResult> DeleteAllUsersFromGroup(int groupId)
        {
            try
            {
                await _groupsUsersService.DeleteAllUsersFromGroupAsync(groupId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("users/{groupId}", Name = "DeleteUsersFromGroup")]
        public async Task<IActionResult> DeleteUsersFromGroup(int[] usersId, int groupId)
        {
            try
            {
                await _groupsUsersService.DeleteUsersFromGroupAsync(usersId, groupId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("all", Name = "GetAllGroupsFromUser")]
        public async Task<ActionResult<List<GetGroupsUsersResponse>>> GetAllGroupsFromUser([FromQuery] GetUsersGroupsPaginationRequest getUsersGroupsPaginationRequest)
        {
            try
            {
                var result = await _groupsUsersService.GetListGroupsUserAsync(getUsersGroupsPaginationRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet(Name = "GetUserWithGroup")]
        public async Task<ActionResult<GetGroupsUsersResponse?>> GetUserWithGroup([FromQuery, Required] int groupId, [FromQuery, Required] int userId)
        {
            try
            {
                var result = await _groupsUsersService.GetUserWithGroup(userId, groupId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("update-group/{groupId}", Name = "UpdateGroupWithUsers")]
        public async Task<IActionResult> UpdateGroupWithUsers(int[] usersId, int groupId)
        {
            try
            {
                await _groupsUsersService.UpdateGroupWithUsersAsync(usersId, groupId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("update-user/{userId}", Name = "UpdateUserWithGroups")]
        public async Task<IActionResult> UpdateUserWithGroups(int[] groupsId, int userId)
        {
            try
            {
                await _groupsUsersService.UpdateUserWithGroupsAsync(groupsId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("permission", Name = "UpdatePermission")]
        public async Task<IActionResult> UpdatePermission(GetUserGroupRequest getUserGroupRequest)
        {
            try
            {
                await _groupsUsersService.UpdatePermission(getUserGroupRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
