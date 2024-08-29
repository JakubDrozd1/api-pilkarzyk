using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.UoW;
using Microsoft.Extensions.Configuration;

namespace BLLLibrary.Service
{
    public class GroupsUsersLinkService(IUnitOfWork unitOfWork, IConfiguration configuration) : IGroupsUsersLinkService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _configuration = configuration;

        public async Task<string> GetLinkAsync(int groupId, int userId)
        {
            var result = await _unitOfWork.ReadGroupsUsersLinkRepository.ReadGroupLinkAsync(groupId, userId);
            return result != null ? GetGroupLink(result.CODE) : "";
        }

        public Task<GROUPS_USERS_LINK?> GetLinkByCodeAsync(string code)
        {
            return _unitOfWork.ReadGroupsUsersLinkRepository.ReadLinkByCodeAsync(code);
        }

        public async Task<string> PostLinkAsync(int groupId, int userId)
        {
            var result = await _unitOfWork.CreateGroupsUsersLinkRepository.CreateGroupLinkAsync(groupId, userId);
            return result != null ? GetGroupLink(result) : "";
        }

        public async Task DeleteLinkAsync(int groupId, int userId)
        {
            await _unitOfWork.DeleteGroupsUsersLinkRepository.DeleteGroupLinkAsync(groupId, userId);
        }

        private string GetGroupLink(string code)
        {
            var angularLink = _configuration.GetSection("Angular");
            return $"{angularLink.Value}/invite/{code}";
        }
    }
}
