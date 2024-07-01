using System.Collections.Generic;
using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Messages;
using DataLibrary.Model.DTO.Request.TableRequest;
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

                var updateColumn = new List<string>();

                updateColumn.Add("ANSWER");
                updateColumn.Add("WAITING_TIME");
                updateColumn.Add("DATE_RESPONSE");

                if (getMessageRequest.ANSWER == "wait")
                {

                    updateColumn.Add("GIVE_ME_A_TIME_CLICKED");

                    DynamicParameters dynamicParametersMeeting = new();

                    dynamicParametersMeeting.Add("@UserId", getMessageRequest.IDUSER);
                    dynamicParametersMeeting.Add("@MeetingId", getMessageRequest.IDMEETING);

                    var query = new QueryBuilder<MESSAGES>()
                        .Select("GIVE_ME_A_TIME_CLICKED ")
                        .From("MESSAGES ")
                        .Where("IDUSER = @UserId AND IDMEETING = @MeetingId");
                    var messages = await _dbConnection.QuerySingleOrDefaultAsync<MESSAGES>(query.Build(), dynamicParametersMeeting, _fbTransaction);

                    if(messages != null && messages.GIVE_ME_A_TIME_CLICKED == true)
                    {
                        throw new Exception("User can not get give a time.");
                    }
                }

                DynamicParameters dynamicParameters = new();
                DateTime currentDateTime = DateTime.Now;
                var updateBuilder = new QueryBuilder<GetMessageRequest>()
                    .UpdateColumns("MESSAGES", updateColumn.ToArray())
                    .Where("IDUSER = @UserId AND IDMEETING = @MeetingId");
                string updateQuery = updateBuilder.Build();
                dynamicParameters.Add("@UserId", getMessageRequest.IDUSER);
                dynamicParameters.Add("@MeetingId", getMessageRequest.IDMEETING);
                dynamicParameters.Add("@ANSWER", getMessageRequest.ANSWER);
                dynamicParameters.Add("@WAITING_TIME", getMessageRequest.WAITING_TIME);
                dynamicParameters.Add("@DATE_RESPONSE", currentDateTime);
                dynamicParameters.Add("@GIVE_ME_A_TIME_CLICKED", getMessageRequest.ANSWER == "wait");
                await _dbConnection.ExecuteAsync(updateQuery, dynamicParameters, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task UpdateTeamMessageAsync(GetTeamMessageRequest getTeamMessageRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                DynamicParameters dynamicParameters = new();
                var updateBuilder = new QueryBuilder<GetMessageRequest>()
                    .UpdateColumns("MESSAGES", ["IDTEAM"])
                    .Where("IDUSER = @UserId AND IDMEETING = @MeetingId");
                string updateQuery = updateBuilder.Build();
                dynamicParameters.Add("@UserId", getTeamMessageRequest.IDUSER);
                dynamicParameters.Add("@MeetingId", getTeamMessageRequest.IDMEETING);
                dynamicParameters.Add("@IDTEAM", getTeamMessageRequest.IDTEAM);
                await _dbConnection.ExecuteAsync(updateQuery, dynamicParameters, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
