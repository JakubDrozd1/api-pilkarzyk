﻿using System.Transactions;
using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using DataLibrary.Repository.GroupsUsers;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class GroupsUsersService(IUnitOfWork unitOfWork) : IGroupsUsersService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task AddUserToGroupAsync(GetUserGroupRequest getUserGroupRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var group = await _unitOfWork.ReadGroupsRepository.GetGroupByIdAsync(getUserGroupRequest.IDGROUP) ?? throw new Exception("Group is null");
                var user = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(getUserGroupRequest.IDUSER) ?? throw new Exception("User is null");
                if (await _unitOfWork.ReadGroupsUsersRepository.GetUserWithGroup(getUserGroupRequest.IDGROUP, getUserGroupRequest.IDUSER) != null)
                {
                    throw new Exception("User is already in this group");
                }
                await _unitOfWork.CreateGroupsUsersRepository.AddUserToGroupAsync(getUserGroupRequest);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                throw new Exception($"{ex.Message}");
            }

        }

        public async Task DeleteAllGroupsFromUser(int userId)
        {
            await _unitOfWork.DeleteGroupsUsersRepository.DeleteAllGroupsFromUser(userId);
        }

        public async Task DeleteAllUsersFromGroupAsync(int groupId)
        {
            await _unitOfWork.DeleteGroupsUsersRepository.DeleteAllUsersFromGroupAsync(groupId);
        }

        public async Task DeleteUsersFromGroupAsync(int[] usersId, int groupId)
        {
            await _unitOfWork.DeleteGroupsUsersRepository.DeleteUsersFromGroupAsync(usersId, groupId);
        }

        public async Task<List<GetGroupsUsersResponse>> GetListGroupsUserAsync(GetUsersGroupsPaginationRequest getUsersGroupsPaginationRequest)
        {
            return await _unitOfWork.ReadGroupsUsersRepository.GetListGroupsUserAsync(getUsersGroupsPaginationRequest);
        }

        public async Task<GetGroupsUsersResponse?> GetUserWithGroup(int groupId, int userId)
        {
            return await _unitOfWork.ReadGroupsUsersRepository.GetUserWithGroup(userId, groupId);
        }

        public async Task UpdateGroupWithUsersAsync(int[] usersId, int groupId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.DeleteGroupsUsersRepository.DeleteAllUsersFromGroupAsync(groupId);
                foreach (int userId in usersId)
                {
                    GetUserGroupRequest getUserGroupRequest = new() { IDGROUP = groupId, IDUSER = userId };
                    await _unitOfWork.CreateGroupsUsersRepository.AddUserToGroupAsync(getUserGroupRequest);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task UpdateUserWithGroupsAsync(int[] groupsId, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.DeleteGroupsUsersRepository.DeleteAllGroupsFromUser(userId);
                foreach (int groupId in groupsId)
                {
                    GetUserGroupRequest getUserGroupRequest = new() { IDGROUP = groupId, IDUSER = userId };
                    await _unitOfWork.CreateGroupsUsersRepository.AddUserToGroupAsync(getUserGroupRequest);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
