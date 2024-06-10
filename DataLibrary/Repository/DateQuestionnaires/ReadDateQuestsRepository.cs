using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.DateQuest;
using DataLibrary.IRepository.Groups;
using DataLibrary.Model.DTO.Request.Pagination;
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

        public async Task<List<DATE_QUESTS?>> GetDateQuestsByMeetingIdAsync(int meetingId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<DATE_QUESTS>()
                    .Select("* ")
                    .From($"{nameof(DATE_QUESTS)} ")
                    .Where("IDMEETING = @MeetingId ");
                return (await _dbConnection.QueryAsync<DATE_QUESTS?>(query.Build(), new { MeetingId = meetingId }, _fbTransaction)).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

    }
}
