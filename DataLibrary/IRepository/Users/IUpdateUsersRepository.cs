﻿using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.Users
{
    public interface IUpdateUsersRepository
    {
        Task UpdateUserAsync(USERS user);
        Task UpdateColumnUserAsync(GetUpdateUserRequest getUpdateUserRequest, int userId, string? salt);
    }
}
