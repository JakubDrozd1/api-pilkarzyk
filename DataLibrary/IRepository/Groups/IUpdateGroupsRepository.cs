﻿using DataLibrary.Entities;

namespace DataLibrary.IRepository.Groups
{
    public interface IUpdateGroupsRepository
    {
        Task UpdateGroupAsync(GROUPS group);
    }
}
