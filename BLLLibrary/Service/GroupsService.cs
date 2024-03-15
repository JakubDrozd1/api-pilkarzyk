using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.UoW;

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

        public async Task AddGroupAsync(GetCreateGroupRequest groupRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var groupTemp = await _unitOfWork.ReadGroupsRepository.GetGroupByNameAsync(groupRequest.GroupRequest.NAME);
                if (groupTemp != null)
                {
                    throw new Exception("Group with this name already exists");
                }
                if (groupRequest.User.IS_ADMIN)
                {
                    await _unitOfWork.CreateGroupsRepository.AddGroupAsync(groupRequest.GroupRequest);
                }
                else
                {
                    if (groupRequest.User.GROUP_COUNTER > 0)
                    {
                        await _unitOfWork.CreateGroupsRepository.AddGroupAsync(groupRequest.GroupRequest);
                        await _unitOfWork.UpdateUsersRepository.UpdateColumnUserAsync(new GetUpdateUserRequest()
                        {
                            Column = ["GROUP_COUNTER"],
                            GROUP_COUNTER = groupRequest.User.GROUP_COUNTER - 1
                        },
                        userId: groupRequest.User.ID_USER,
                        salt: groupRequest.User.SALT
                        );
                        var addedGroup = await _unitOfWork.ReadGroupsRepository.GetGroupByNameAsync(groupRequest.GroupRequest.NAME);
                        await _unitOfWork.CreateGroupsUsersRepository.AddUserToGroupAsync(new GetUserGroupRequest()
                        {
                            IDGROUP = addedGroup?.ID_GROUP ?? throw new Exception("Group is null"),
                            IDUSER = groupRequest.User.ID_USER,
                            ACCOUNT_TYPE = 1
                        });
                    }
                    else
                    {
                        throw new Exception("Group conter < 1");
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

        public async Task UpdateGroupAsync(GetGroupRequest groupRequest, int groupId)
        {
            GROUPS group = new()
            {
                ID_GROUP = groupId,
                NAME = groupRequest.NAME
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
