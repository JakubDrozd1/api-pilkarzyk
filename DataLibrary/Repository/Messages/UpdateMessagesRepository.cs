using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Messages;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Messages
{
    public class UpdateMessagesRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IUpdateMessagesRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task UpdateMessageAsync(MESSAGES message)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var updateBuilder = new QueryBuilder<MESSAGES>()
                    .Update("MESSAGES ", message)
                    .Where("ID_MESSAGE = @ID_MESSAGE ");
                string updateQuery = updateBuilder.Build();
                await _dbConnection.ExecuteAsync(updateQuery, message, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task UpdateAnswerMessageAsync(GetMessageRequest getMessageRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                DynamicParameters dynamicParameters = new();
                DateTime currentDateTime = DateTime.Now;
                var updateBuilder = new QueryBuilder<GetMessageRequest>()
                    .UpdateColumns("MESSAGES", ["ANSWER", "WAITING_TIME", "DATE_RESPONSE"])
                    .Where("IDUSER = @UserId AND IDMEETING = @MeetingId");
                string updateQuery = updateBuilder.Build();
                dynamicParameters.Add("@UserId", getMessageRequest.IDUSER);
                dynamicParameters.Add("@MeetingId", getMessageRequest.IDMEETING);
                dynamicParameters.Add("@ANSWER", getMessageRequest.ANSWER);
                dynamicParameters.Add("@WAITING_TIME", getMessageRequest.WAITING_TIME);
                dynamicParameters.Add("@DATE_RESPONSE", currentDateTime);
                await _dbConnection.ExecuteAsync(updateQuery, dynamicParameters, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
