using System.Data;
using System.Data.Common;
using Dapper;
using DataLibrary.Helper;
using DataLibrary.IRepository.Meetings;
using DataLibrary.Model.DTO.Request.TableRequest;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Meetings
{
    public class CreateMeetingsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task<int> AddMeetingAsync(GetMeetingRequest getMeetingRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var insertBuilder = new QueryBuilder<GetMeetingRequest>()
                    .Insert("MEETINGS ", getMeetingRequest);
                string insertQuery = insertBuilder.Build();

                insertQuery += "returning ID_MEETING";

                DynamicParameters dynamicParameters = new();

                dynamicParameters.Add("@DATE_MEETING", getMeetingRequest.DATE_MEETING);
                dynamicParameters.Add("@PLACE", getMeetingRequest.PLACE);
                dynamicParameters.Add("@QUANTITY", getMeetingRequest.QUANTITY);
                dynamicParameters.Add("@DESCRIPTION", getMeetingRequest.DESCRIPTION);
                dynamicParameters.Add("@IDGROUP", getMeetingRequest.IDGROUP);
                dynamicParameters.Add("@IDAUTHOR", getMeetingRequest.IDAUTHOR);
                dynamicParameters.Add("@IS_INDEPENDENT", getMeetingRequest.IS_INDEPENDENT);
                dynamicParameters.Add("@WAITING_TIME_DECISION", getMeetingRequest.WAITING_TIME_DECISION);
                dynamicParameters.Add("@DATE_QUEST_OPEN", getMeetingRequest.DATE_QUEST_OPEN);
                dynamicParameters.Add("@IS_QUEST", getMeetingRequest.IS_QUEST);
                dynamicParameters.Add("@DATE_QUEST_END", getMeetingRequest.DATE_QUEST_END);
                dynamicParameters.Add("@MAX_GIVE_ME_TIME", getMeetingRequest.MAX_GIVE_ME_TIME);
                dynamicParameters.Add("@REMINDER_MESSAGES_TIME", getMeetingRequest.REMINDER_MESSAGES_TIME);
                dynamicParameters.Add("@LAST_REMINDER_MESSAGES_TIME", DateTime.Now);


                var result = await _dbConnection.QueryAsync<int>(insertQuery, dynamicParameters, _fbTransaction);

                return result.Single();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
