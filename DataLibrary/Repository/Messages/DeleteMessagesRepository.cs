﻿using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Messages;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Messages
{
    public class DeleteMessagesRepository(FbConnection dbConnection) : IDeleteMessagesRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task DeleteMessageAsync(int messageId, FbTransaction? transaction = null)
        {
            var deleteBuilder = new QueryBuilder<MESSAGES>()
                .Delete("MESSAGES ")
                .Where("ID_MESSAGE = @MessageId ");
            string deleteQuery = deleteBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(deleteQuery, new { MessageId = messageId }, transaction);
        }
    }
}
