using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
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
                var meetings = await _unitOfWork.ReadMeetingsRepository.GetAllMeetingsAsync(new GetMeetingsGroupsPaginationRequest()
                {
                    OnPage = -1,
                    Page = 0,
                    DateFrom = DateTime.Now,
                    IdGroup = getUserGroupRequest.IDGROUP,
                    WithMessages = false,
                });
                if (meetings != null)
                {
                    foreach (var meeting in meetings)
                    {
                        await _unitOfWork.CreateUsersMeetingRepository.AddUserToMeetingAsync(meeting, getUserGroupRequest.IDUSER);
                        await _unitOfWork.CreateMessagesRepository.AddMessageAsync(new GetMessageRequest()
                        {
                            IDUSER = getUserGroupRequest.IDUSER,
                            IDMEETING = meeting.IdMeeting
                        });
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
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
                await _unitOfWork.RollBackTransactionAsync();
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
                await _unitOfWork.RollBackTransactionAsync();
                throw new Exception($"{ex.Message}");
            }
        }
        public async Task UpdatePermission(GetUserGroupRequest getUserGroupRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var group = await _unitOfWork.ReadGroupsRepository.GetGroupByIdAsync(getUserGroupRequest.IDGROUP) ?? throw new Exception("Group is null");
                var user = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(getUserGroupRequest.IDUSER) ?? throw new Exception("User is null");
                var groupUser = await _unitOfWork.ReadGroupsUsersRepository.GetUserWithGroup(getUserGroupRequest.IDGROUP, getUserGroupRequest.IDUSER) ?? throw new Exception("User is not in group");
                GROUPS_USERS groupUserTemp = new()
                {
                    IDGROUP = groupUser.IdGroup,
                    IDUSER = groupUser.IdUser,
                    ACCOUNT_TYPE = getUserGroupRequest.ACCOUNT_TYPE,
                };
                await _unitOfWork.UpdateGroupUsersRepository.UpdateGroupUserAsync(groupUserTemp);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
