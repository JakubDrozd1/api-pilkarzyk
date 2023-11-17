using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface IGroupsRepository
    {
        List<Groupe> GetAllGroups();
        Groupe? GetGroupById(int groupId);
        void AddGroup(Groupe group);
        void UpdateGroup(Groupe group);
        void DeleteGroup(int groupId);
    }
}
