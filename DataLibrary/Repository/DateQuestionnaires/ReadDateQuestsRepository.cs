using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.DateQuest;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.DateQuests
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

        public async Task<List<DATE_QUESTS>> GetDateQuestsByMeetingIdAsync(int meetingId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GetDateQuestsResponse>()
                    .Select("dq.ID_DATE_QUEST ,dq.DATE_MEETING, dq.IDMEETING, dq.DATE_MEETING, udq.IDDATE_QUESTS, udq.IDUSER ")
                    .From($"{nameof(DATE_QUESTS)} dq " +
                          $"LEFT JOIN {nameof(USERS_DATE_QUESTS)} udq ON dq.{nameof(DATE_QUESTS.ID_DATE_QUEST)} = udq.{nameof(USERS_DATE_QUESTS.IDDATE_QUESTS)}")
                    .Where("IDMEETING = @MeetingId ");

                var dateQuests = (await _dbConnection.QueryAsync<DATE_QUESTS, USERS_DATE_QUESTS, DATE_QUESTS>(query.Build(), (dateQuest, userDateQuest) =>
                {
                    if (userDateQuest != null)
                    {
                        dateQuest.USERS_DATE_QUESTS.Add(userDateQuest);
                    }
                    return dateQuest;
                },
                    new { MeetingId = meetingId },
                    _fbTransaction,
                    splitOn: "IDUSER"
                    ))
                    .AsList();

                var result = dateQuests.GroupBy(dateQuest => dateQuest.ID_DATE_QUEST).Select(dateQuest =>
                {
                    var grupedDateQuest = dateQuest.First();
                    grupedDateQuest.USERS_DATE_QUESTS = dateQuest
                    .Where(userDateQuests => userDateQuests.USERS_DATE_QUESTS.SingleOrDefault() != null)
                    .Select(userDateQuests => userDateQuests.USERS_DATE_QUESTS.SingleOrDefault())
                    .ToList()!;

                    return grupedDateQuest;
                }).ToList();

                return result;

            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public List<DATE_QUESTS> GetDateQuestByMeetingId(int meetingId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GetDateQuestsResponse>()
                    .Select("dq.ID_DATE_QUEST ,dq.DATE_MEETING, dq.IDMEETING, dq.DATE_MEETING, udq.IDDATE_QUESTS, udq.IDUSER ")
                    .From($"{nameof(DATE_QUESTS)} dq " +
                          $"LEFT JOIN {nameof(USERS_DATE_QUESTS)} udq ON dq.{nameof(DATE_QUESTS.ID_DATE_QUEST)} = udq.{nameof(USERS_DATE_QUESTS.IDDATE_QUESTS)}")
                    .Where("IDMEETING = @MeetingId ");
                var dateQuests = (_dbConnection.Query<DATE_QUESTS, USERS_DATE_QUESTS, DATE_QUESTS>(query.Build(), (dateQuest, userDateQuest) =>
                    {
                        if (userDateQuest != null)
                        {
                            dateQuest.USERS_DATE_QUESTS.Add(userDateQuest);
                        }
                        return dateQuest;
                    },
                    new { MeetingId = meetingId },
                    _fbTransaction,
                    splitOn: "IDUSER"
                    ))
                .AsList();

                var result = dateQuests.GroupBy(dateQuest => dateQuest.ID_DATE_QUEST).Select(dateQuest =>
                {
                    var grupedDateQuest = dateQuest.First();
                    grupedDateQuest.USERS_DATE_QUESTS = dateQuest
                    .Where(userDateQuests => userDateQuests.USERS_DATE_QUESTS.SingleOrDefault() != null)
                    .Select(userDateQuests => userDateQuests.USERS_DATE_QUESTS.SingleOrDefault())
                    .ToList();

                    return grupedDateQuest;
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

    }
}
