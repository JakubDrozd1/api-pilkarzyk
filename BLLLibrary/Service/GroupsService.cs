using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    internal class GroupsService(IUnitOfWork unitOfWork) : IGroupsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<Groupe>> GetAllGroupsAsync()
        {
            return await _unitOfWork.ReadGroupsRepository.GetAllGroupsAsync();
        }

        public async Task<Groupe?> GetGroupByIdAsync(int groupId)
        {
            return await _unitOfWork.ReadGroupsRepository.GetGroupByIdAsync(groupId);
        }

        public async Task AddGroupAsync(Groupe group)
        {
            await _unitOfWork.CreateGroupsRepository.AddGroupAsync(group);
        }

        public async Task UpdateGroupAsync(Groupe group)
        {
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
    }
}
