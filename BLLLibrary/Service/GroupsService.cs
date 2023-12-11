using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.UoW;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.Service
{
    public class GroupsService(IUnitOfWork unitOfWork) : IGroupsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<GROUPS>> GetAllGroupsAsync(GetGroupsPaginationRequest getGroupsPaginationRequest)
        {
            return await _unitOfWork.ReadGroupsRepository.GetAllGroupsAsync(getGroupsPaginationRequest);
        }

        public async Task<GROUPS?> GetGroupByIdAsync(int groupId)
        {
            return await _unitOfWork.ReadGroupsRepository.GetGroupByIdAsync(groupId);
        }

        public async Task AddGroupAsync(GetGroupRequest groupRequest)
        {
            GROUPS group = new()
            {
                NAME = groupRequest.Name
            };
            await _unitOfWork.CreateGroupsRepository.AddGroupAsync(group);
        }

        public async Task UpdateGroupAsync(GetGroupRequest groupRequest, int groupId)
        {
            GROUPS group = new()
            {
                ID_GROUP = groupId,
                NAME = groupRequest.Name
            };
            await _unitOfWork.UpdateGroupsRepository.UpdateGroupAsync(group);
        }

        public async Task DeleteGroupAsync(int groupId)
        {
            await _unitOfWork.DeleteGroupsRepository.DeleteGroupAsync(groupId);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }

        public Task<GROUPS?> GetGroupByNameAsync(string name)
        {
            return _unitOfWork.ReadGroupsRepository.GetGroupByNameAsync(name);
        }
    }
}
