﻿using DataLibrary.Entities;

namespace DataLibrary.IRepository.Groups
{
    public interface IReadGroupsRepository
    {
        Task<List<Groupe>> GetAllGroupsAsync();
        Task<Groupe?> GetGroupByIdAsync(int groupId);
    }
}
