using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Helper.Notification;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
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
                var invites = await _unitOfWork.ReadGroupInviteRepository.GetGroupInviteByIdUserAsync(
                    new GetGroupInvitePaginationRequest()
                    {
                        OnPage = -1,
                        Page = 0,
                        IdGroup = getGroupInviteRequest.IDGROUP,
                        IdUser = getGroupInviteRequest.IDUSER
                    }
                    );
                if (invites.Count > 0) throw new Exception("Invitation alredy send");
                await _unitOfWork.CreateGroupInviteRepository.AddGroupInviteAsync(getGroupInviteRequest);
                await _unitOfWork.SaveChangesAsync();
                await SendNotificationToUserAsync(group, user.ID_USER);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw new Exception($"{ex.Message}");
            }

        }

        private async Task SendNotificationToUserAsync(GROUPS group, int idUser)
        {
            FirebaseNotification notificationHub = new();

            var tokens = await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(idUser);

            if (tokens != null)
            {
                notificationHub.SendGroupNotification(group, tokens);
            }
        }

        public async Task DeleteGroupInviteAsync(int groupInviteId)
        {
            await _unitOfWork.DeleteGroupInviteRepository.DeleteGroupInviteAsync(groupInviteId);
        }

        public async Task<List<GetGroupInviteResponse?>> GetGroupInviteByIdUserAsync(GetGroupInvitePaginationRequest getGroupInvitePaginationRequest)
        {
            return await _unitOfWork.ReadGroupInviteRepository.GetGroupInviteByIdUserAsync(getGroupInvitePaginationRequest);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
