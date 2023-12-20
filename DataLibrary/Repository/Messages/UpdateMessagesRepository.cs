using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Messages;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Repository.Users;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Messages
{
    public class UpdateMessagesRepository(FbConnection dbConnection) : IUpdateMessagesRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateMessageAsync(MESSAGES message, FbTransaction? transaction = null)
        {
            var updateBuilder = new QueryBuilder<MESSAGES>()
               .Update("MESSAGES ", message)
               .Where("ID_MESSAGE = @ID_MESSAGE ");
            string updateQuery = updateBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(updateQuery, message, transaction);
        }

        public async Task UpdateAnswerMessageAsync(GetMessageRequest getMessageRequest, FbTransaction? transaction = null)
        {
            DynamicParameters dynamicParameters = new();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            ReadUsersRepository readUsersRepository = new(db);

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            var localTransaction = transaction ?? await db.BeginTransactionAsync();
            try
            {
                var updateBuilder = new QueryBuilder<GetMessageRequest>()
                    .UpdateColumns("MESSAGES", ["ANSWER"])
                    .Where("IDUSER = @UserId AND IDMEETING = @MeetingId");
                string updateQuery = updateBuilder.Build();
                dynamicParameters.Add("@UserId", getMessageRequest.IdUser);
                dynamicParameters.Add("@MeetingId", getMessageRequest.IdMeeting);
                dynamicParameters.Add("@ANSWER", getMessageRequest.Answer);
                await db.ExecuteAsync(updateQuery, dynamicParameters, localTransaction);

                if (transaction == null)
                {
                    await localTransaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                localTransaction?.RollbackAsync();
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
