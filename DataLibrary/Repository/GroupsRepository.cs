using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataLibrary.IRepository;
using Dapper;
using DataLibrary.Entities;

namespace DataLibrary.Repository
{
    internal class GroupsRepository : IGroupsRepository
    {
        private readonly string connectionString;

        public GroupsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Groupe> GetAllGroups()
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                return dbConnection.Query<Groupe>("SELECT * FROM Groups").ToList();
            }
        }

        public Groupe? GetGroupById(int groupId)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                return dbConnection.QueryFirstOrDefault<Groupe>("SELECT * FROM Groups WHERE IdGroup = @GroupId", new { GroupId = groupId });
            }
        }

        public void AddGroup(Groupe group)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("INSERT INTO Groups (Name) VALUES (@Name)", group);
            }
        }

        public void UpdateGroup(Groupe group)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("UPDATE Groups SET Name = @Name WHERE IdGroup = @IdGroup", group);
            }
        }

        public void DeleteGroup(int groupId)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("DELETE FROM Groups WHERE IdGroup = @GroupId", new { GroupId = groupId });
            }
        }
    }
}
