using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Helper.Email;
using DataLibrary.Helper.Notification;
using DataLibrary.Model.DTO.Request.EmailRequest;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;
using Microsoft.Extensions.Configuration;

namespace BLLLibrary.Service
{
    public class GroupInviteService(IUnitOfWork unitOfWork, IConfiguration configuration) : IGroupInviteService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _configuration = configuration;

        public async Task AddGroupInviteAsync(GetGroupInviteRequest getGroupInviteRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userEmail = await _unitOfWork.ReadUsersRepository.GetUserByEmailAsync(getGroupInviteRequest.EMAIL);
                if (userEmail != null)
                {
                    var group = await _unitOfWork.ReadGroupsRepository.GetGroupByIdAsync(getGroupInviteRequest.IDGROUP) ?? throw new Exception("Group is null");
                    var user = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(userEmail.ID_USER);
                    if (await _unitOfWork.ReadGroupsUsersRepository.GetUserWithGroup(getGroupInviteRequest.IDGROUP, userEmail.ID_USER) != null)
                    {
                        throw new Exception("User is already in this group");
                    }
                    var invites = await _unitOfWork.ReadGroupInviteRepository.GetGroupInviteByIdUserAsync(
                    new GetGroupInvitePaginationRequest()
                    {
                        OnPage = -1,
                        Page = 0,
                        IdGroup = getGroupInviteRequest.IDGROUP,
                        IdUser = userEmail.ID_USER
                    });
                    if (invites.Count > 0) throw new Exception("Invitation alredy send");
                    getGroupInviteRequest.IDUSER = userEmail.ID_USER;
                    await _unitOfWork.CreateGroupInviteRepository.AddGroupInviteAsync(getGroupInviteRequest);
                    await _unitOfWork.SaveChangesAsync();
                    await SendNotificationToUserAsync(group, userEmail.ID_USER);
                }
                else
                {
                    var group = await _unitOfWork.ReadGroupsRepository.GetGroupByIdAsync(getGroupInviteRequest.IDGROUP) ?? throw new Exception("Group is null");
                    var author = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(getGroupInviteRequest.IDAUTHOR) ?? throw new Exception("Author is null");
                    var invites = await _unitOfWork.ReadGroupInviteRepository.GetGroupInviteByIdUserAsync(
                        new GetGroupInvitePaginationRequest()
                        {
                            OnPage = -1,
                            Page = 0,
                            IdGroup = getGroupInviteRequest.IDGROUP,
                            Email = getGroupInviteRequest.EMAIL
                        });
                    if (invites.Count > 0) throw new Exception("Invitation alredy send");
                    await _unitOfWork.CreateGroupInviteRepository.AddGroupInviteAsync(getGroupInviteRequest);
                    var invite = await _unitOfWork.ReadGroupInviteRepository.GetLastAddedInvite(getGroupInviteRequest) ?? throw new Exception("Invite not found");
                    await _unitOfWork.SaveChangesAsync();

                    string? email = _configuration["MailSettings:From"] ?? throw new Exception("Sender not found");
                    EMAIL_SENDER? emailSender = await _unitOfWork.ReadEmailSender.GetEmailDetailsAsync(email) ?? throw new Exception("Sender not found");
                    EmailSender sendmail = new(emailSender, new CancellationToken());
                    var result = await sendmail.SendInviteMessageAsync(new GetEmailInvitationGroupRequest()
                    {
                        GroupName = group.NAME,
                        To = getGroupInviteRequest.EMAIL,
                        Name = author.FIRSTNAME,
                        Surname = author.SURNAME,
                        IdGroupInvite = invite.ID_GROUP_INVITE ?? throw new Exception("Sender not found"),
                    });
                    if (!result)
                    {
                        throw new Exception("Mails not send");
                    }
                }
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
                await notificationHub.SendGroupNotification(group, tokens);
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

        public async Task<GROUP_INVITE?> GetGroupInviteByIdAsync(int groupInviteId)
        {
            return await _unitOfWork.ReadGroupInviteRepository.GetGroupInviteByIdAsync(groupInviteId);
        }
    }
}
