﻿using Dapper;
using System.Data;
using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.IRepository.Tokens;
using DataLibrary.Helper;

namespace DataLibrary.Repository.Tokens
{
    public class ReadTokensRepository(FbConnection dbConnection) : IReadTokensRepository
    {
        private readonly FbConnection dbConnection = dbConnection;
        public async Task<ACCESS_TOKENS?> GetTokenByUserAsync(int userId, FbTransaction? transaction = null)
        {

            var query = new QueryBuilder<ACCESS_TOKENS>()
                .Select("* ")
                .From("ACCESS_TOKENS ")
                .Where("IDUSER = @UserId ");
            FbConnection db = transaction?.Connection ?? dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return await db.QuerySingleOrDefaultAsync<ACCESS_TOKENS>(query.Build(), new { UserId = userId }, transaction);
        }
    }
}
