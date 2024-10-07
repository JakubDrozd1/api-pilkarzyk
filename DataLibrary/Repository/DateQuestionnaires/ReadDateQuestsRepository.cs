using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.DateQuest;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.DateQuestionnaires
{
    public class ReadDateQuestsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadDateQuestsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task<DATE_QUESTS?> GetDateQuestByIdAsync(int dateQuestsId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<DATE_QUESTS>()
                    .Select("* ")
                    .From("DATE_QUESTS ")
                    .Where("ID_DATE_QUEST = @DateQuestsId ");
                return await _dbConnection.QuerySingleOrDefaultAsync<DATE_QUESTS>(query.Build(), new { DateQuestsId = dateQuestsId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<List<GetDateQuestsResponse>> GetDateQuestsByMeetingIdAsync(int meetingId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GetDateQuestsResponse>()
                    .Select("dq.DATE_MEETING AS DateMeeting, " +
                            "dq.ID_DATE_QUEST AS IdDateQuest, " +
                            "udq.DATE_SELECTION AS DateSelection, " +
                            "u.ID_USER AS IdUser, " +
                            "u.AVATAR, " +
                            "u.LOGIN, " +
                            "u.FIRSTNAME, " +
                            "u.SURNAME, " +
                            "u.EMAIL ")
                    .From($"{nameof(DATE_QUESTS)} dq " +
                          $"LEFT JOIN {nameof(USERS_DATE_QUESTS)} udq ON dq.{nameof(DATE_QUESTS.ID_DATE_QUEST)} = udq.{nameof(USERS_DATE_QUESTS.IDDATE_QUESTS)} " +
                          $"LEFT JOIN {nameof(USERS)} u ON udq.{nameof(USERS_DATE_QUESTS.IDUSER)} = u.ID_USER ")
                    .Where("dq.IDMEETING = @MeetingId");

                var dateQuestsDictionary = new Dictionary<(DateTime DateMeeting, int IdDateQuest), GetDateQuestsResponse>();

                var result = await _dbConnection.QueryAsync<GetDateQuestsResponse, GetArrayUsersResponse, GetDateQuestsResponse>(
                    query.Build(),
                        (dateQuest, user) =>
                        {


                            var key = (dateQuest.DateMeeting, dateQuest.IdDateQuest);

                            if (!dateQuestsDictionary.TryGetValue(key, out var existingDateQuest))
                            {
                                existingDateQuest = new GetDateQuestsResponse
                                {
                                    DateMeeting = dateQuest.DateMeeting,
                                    IdDateQuest = dateQuest.IdDateQuest,
                                    Users = []
                                };
                                dateQuestsDictionary.Add(key, existingDateQuest);
                            }
                            if (user != null)
                            {
                                existingDateQuest.Users.Add(user);
                            }

                            return existingDateQuest;
                        },
                        new { MeetingId = meetingId },
                        _fbTransaction,
                        splitOn: "DateSelection"
                );

                return [.. dateQuestsDictionary.Values];
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }


    }
}
