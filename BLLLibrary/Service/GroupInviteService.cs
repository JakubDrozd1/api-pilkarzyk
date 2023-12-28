using BLLLibrary.IService;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class GroupInviteService(IUnitOfWork unitOfWork) : IGroupInviteService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task AddGroupInviteAsync(GetGroupInviteRequest getGroupInviteRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var group = await _unitOfWork.ReadGroupsRepository.GetGroupByIdAsync(getGroupInviteRequest.IDGROUP) ?? throw new Exception("Group is null");
                var user = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(getGroupInviteRequest.IDUSER) ?? throw new Exception("User is null");
                if (await _unitOfWork.ReadGroupsUsersRepository.GetUserWithGroup(getGroupInviteRequest.IDGROUP, getGroupInviteRequest.IDUSER) != null)
                {
                    throw new Exception("User is already in this group");
                }
                await _unitOfWork.CreateGroupInviteRepository.AddGroupInviteAsync(getGroupInviteRequest);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                throw new Exception($"{ex.Message}");
            }

        }

        public async Task DeleteGroupInviteAsync(int groupInviteId)
        {
            await _unitOfWork.DeleteGroupInviteRepository.DeleteGroupInviteAsync(groupInviteId);
        }

        public async Task<List<GetGroupInviteResponse?>> GetGroupInviteByIdUserAsync(int userId)
        {
            return await _unitOfWork.ReadGroupInviteRepository.GetGroupInviteByIdUserAsync(userId);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
