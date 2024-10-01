using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.GroupsLink;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace DataLibrary.Repository.GroupsLink
{
    public class CreateGroupsUsersLinkRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateGroupsUsersLinkRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task<string> CreateGroupLinkAsync(int groupId, int userId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var newCode = GenerateCode();
                var insertObject = new GROUPS_USERS_LINK
                {
                    CODE = newCode,
                    IDGROUP = groupId,
                    IDUSER = userId,
                };
                var insertBuilder = new QueryBuilder<GROUPS_USERS_LINK>()
                    .Insert("GROUPS_USERS_LINK ", insertObject);
                string insertQuery = insertBuilder.Build();
                await _dbConnection.ExecuteAsync(insertQuery, insertObject, _fbTransaction);
                return newCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        private static string GenerateCode(int length = 20)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            using (var crypto = new RNGCryptoServiceProvider())
            {
                var data = new byte[length];
                crypto.GetBytes(data);
                var result = new StringBuilder(length);
                foreach (byte b in data)
                {
                    result.Append(chars[b % chars.Length]);
                }
                return result.ToString();
            }
        }
    }
}
