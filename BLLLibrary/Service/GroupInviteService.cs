using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Helper.Email;
using DataLibrary.Helper.Notification;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.EmailRequest;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit.Cryptography;

namespace BLLLibrary.Service
{
    public class GroupInviteService(IUnitOfWork unitOfWork, IConfiguration configuration) : IGroupInviteService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _configuration = configuration;

        public async Task AddGroupInviteAsync(GetGroupInviteWithEmailOrPhoneRequest getGroupInviteRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var group = await _unitOfWork.ReadGroupsRepository.GetGroupByIdAsync(getGroupInviteRequest.IDGROUP) ?? throw new Exception("Group is null");
                var author = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(getGroupInviteRequest.IDAUTHOR) ?? throw new Exception("Author is null");

                var isEmail = !(getGroupInviteRequest.EMAIL_OR_PHONE_NUMBER?.Length == 9 && int.TryParse(getGroupInviteRequest.EMAIL_OR_PHONE_NUMBER, out _));

                if (isEmail)
                {
                    var userEmail = await _unitOfWork.ReadUsersRepository.GetUserByEmailAsync(getGroupInviteRequest.EMAIL_OR_PHONE_NUMBER);
                    if (userEmail != null)
                    {

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
                        if (user?.SEND_INVITE ?? false)
                        {
                            getGroupInviteRequest.IDUSER = userEmail.ID_USER;
                            getGroupInviteRequest.PHONE_NUMBER = user.PHONE_NUMBER;
                            getGroupInviteRequest.EMAIL = userEmail.EMAIL;
                            await _unitOfWork.CreateGroupInviteRepository.AddGroupInviteAsync(getGroupInviteRequest);
                            await _unitOfWork.SaveChangesAsync();
                            await SendNotificationToUserAsync(group, userEmail.ID_USER);
                        }
                        else
                        {
                            await AddUserToGroup(userEmail.ID_USER, getGroupInviteRequest.IDGROUP);
                            await _unitOfWork.SaveChangesAsync();
                            await SendNotificationAddUserToGroupAsync(group, userEmail.ID_USER, author);

                        }
                    }
                    else
                    {
                        var invites = await _unitOfWork.ReadGroupInviteRepository.GetGroupInviteByIdUserAsync(
                            new GetGroupInvitePaginationRequest()
                            {
                                OnPage = -1,
                                Page = 0,
                                IdGroup = getGroupInviteRequest.IDGROUP,
                                Email = getGroupInviteRequest.EMAIL_OR_PHONE_NUMBER
                            });
                        if (invites.Count > 0) throw new Exception("Invitation alredy send");

                        var newGetGroupInviteRequest = new GetGroupInviteRequest
                        {
                            IDUSER = getGroupInviteRequest.IDUSER,
                            IDAUTHOR = getGroupInviteRequest.IDAUTHOR,
                            EMAIL = getGroupInviteRequest.EMAIL_OR_PHONE_NUMBER,
                            IDGROUP = getGroupInviteRequest.IDGROUP,

                        };

                        await _unitOfWork.CreateGroupInviteRepository.AddGroupInviteAsync(newGetGroupInviteRequest);
                        var invite = await _unitOfWork.ReadGroupInviteRepository.GetLastAddedInvite(newGetGroupInviteRequest) ?? throw new Exception("Invite not found");
                        await _unitOfWork.SaveChangesAsync();

                        string? email = _configuration["MailSettings:From"] ?? throw new Exception("Sender not found");
                        EMAIL_SENDER? emailSender = await _unitOfWork.ReadEmailSender.GetEmailDetailsAsync(email) ?? throw new Exception("Sender not found");
                        EmailSender sendmail = new(emailSender, _configuration, new CancellationToken());
                        var result = await sendmail.SendInviteMessageAsync(new GetEmailInvitationGroupRequest()
                        {
                            GroupName = group.NAME,
                            To = newGetGroupInviteRequest.EMAIL,
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
                else
                {
                    var PhoneNumber = 0;
                    try
                    {
                        PhoneNumber = int.Parse(getGroupInviteRequest.EMAIL_OR_PHONE_NUMBER);

                    }catch(Exception ex)
                    {
                        throw new Exception("Phone number is null");
                    }

                    var user = await _unitOfWork.ReadUsersRepository.GetUserByPhoneNumberAsync(PhoneNumber) ?? throw new Exception("User with this phone number dont exist");

                    if (await _unitOfWork.ReadGroupsUsersRepository.GetUserWithGroup(getGroupInviteRequest.IDGROUP, user.ID_USER) != null)
                    {
                        throw new Exception("User is already in this group");
                    }
                    var invites = await _unitOfWork.ReadGroupInviteRepository.GetGroupInviteByIdUserAsync(
                    new GetGroupInvitePaginationRequest()
                    {
                        OnPage = -1,
                        Page = 0,
                        IdGroup = getGroupInviteRequest.IDGROUP,
                        IdUser = user.ID_USER
                    });
                    if (invites.Count > 0) throw new Exception("Invitation alredy send");
                    if (user?.SEND_INVITE ?? false)
                    {
                        getGroupInviteRequest.IDUSER = user.ID_USER;
                        getGroupInviteRequest.PHONE_NUMBER = user.PHONE_NUMBER;
                        getGroupInviteRequest.EMAIL = user.EMAIL;
                        await _unitOfWork.CreateGroupInviteRepository.AddGroupInviteAsync(getGroupInviteRequest);
                        await _unitOfWork.SaveChangesAsync();
                        await SendNotificationToUserAsync(group, user.ID_USER);
                    }
                    else
                    {
                        await AddUserToGroup(user?.ID_USER ?? throw new Exception("User is null"), getGroupInviteRequest.IDGROUP);
                        await _unitOfWork.SaveChangesAsync();
                        await SendNotificationAddUserToGroupAsync(group, user.ID_USER, author);
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
            var userDetails = await _unitOfWork.ReadNotificationRepository.GetAllNotificationFromUser(idUser);
            if (tokens != null && userDetails != null)
            {
                if (userDetails.GROUP_INV_NOTIFICATION)
                {
                    await notificationHub.SendGroupNotification(group, tokens);
                }
            }
        }

        private async Task SendNotificationAddUserToGroupAsync(GROUPS group, int idUser, USERS author)
        {
            FirebaseNotification notificationHub = new();

            var tokens = await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(idUser);
            var userDetails = await _unitOfWork.ReadNotificationRepository.GetAllNotificationFromUser(idUser);
            if (tokens != null && userDetails != null)
            {
                if (userDetails.GROUP_ADD_NOTIFICATION)
                {
                    await notificationHub.SendGroupAddUserNotification(group, tokens, author);
                }
            }
        }
        private async Task AddUserToGroup(int idUser, int idGroup)
        {
            await _unitOfWork.CreateGroupsUsersRepository.AddUserToGroupAsync(new GetUserGroupRequest()
            {
                IDGROUP = idGroup,
                IDUSER = idUser,
                ACCOUNT_TYPE = 0
            });
            var meetings = await _unitOfWork.ReadMeetingsRepository.GetAllMeetingsAsync(new GetMeetingsGroupsPaginationRequest()
            {
                OnPage = -1,
                Page = 0,
                DateFrom = DateTime.Now,
                IdGroup = idGroup,
                WithMessages = false,
            });
            if (meetings != null)
            {
                foreach (var meeting in meetings)
                {
                    await _unitOfWork.CreateUsersMeetingRepository.AddUserToMeetingAsync(meeting, idUser);
                    await _unitOfWork.CreateMessagesRepository.AddMessageAsync(new GetMessageRequest()
                    {
                        IDUSER = idUser,
                        IDMEETING = meeting.IdMeeting
                    });
                }
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

        public async Task AddMultipleGroupInviteAsync(GetMultipleGroupInviteRequest getMultipleGroupInviteRequest)
        {
            if (getMultipleGroupInviteRequest.PhoneNumbers != null)
            {
                foreach (var number in getMultipleGroupInviteRequest.PhoneNumbers)
                {
                    var getGroupInviteRequest = new GetGroupInviteWithEmailOrPhoneRequest()
                    {
                        IDAUTHOR = getMultipleGroupInviteRequest.IdAuthor,
                        IDGROUP = getMultipleGroupInviteRequest.IdGroup,
                        EMAIL_OR_PHONE_NUMBER = $"{number}"
                    };
                    await AddGroupInviteAsync(getGroupInviteRequest);
                }
            }
        }
    }
}
