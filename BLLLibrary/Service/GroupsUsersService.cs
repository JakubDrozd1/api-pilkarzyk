using BLLLibrary.IService;
using BLLLibrary.Model.DTO.Response;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class GroupsUsersService(IUnitOfWork unitOfWork) : IGroupsUsersService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task AddUserToGroupAsync(int userId, int groupId)
        {
            await _unitOfWork.CreateGroupsUsersRepository.AddUserToGroupAsync(userId, groupId);
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

        public async Task<List<GetGroupsWithUsersResponse>> GetAllGroupsFromUserAsync(int userId)
        {
            return await _unitOfWork.ReadGroupsUsersRepository.GetAllGroupsFromUserAsync(userId);
        }

        public async Task<List<GetGroupsWithUsersResponse>> GetAllUsersFromGroupAsync(int groupId)
        {
            return await _unitOfWork.ReadGroupsUsersRepository.GetAllUsersFromGroupAsync(groupId);
        }

        public async Task<GetGroupsWithUsersResponse?> GetUserWithGroup(int groupId, int userId)
        {
           return await _unitOfWork.ReadGroupsUsersRepository.GetUserWithGroup(userId, groupId);
        }

        public async Task UpdateGroupWithUsersAsync(int[] usersId, int groupId)
        {
            await _unitOfWork.UpdateGroupsUsersRepository.UpdateGroupWithUsersAsync(usersId, groupId);
        }

        public async Task UpdateUserWithGroupsAsync(int[] groupsId, int userId)
        {
            await _unitOfWork.UpdateGroupsUsersRepository.UpdateUserWithGroupsAsync(groupsId, userId);
        }
    }
}
