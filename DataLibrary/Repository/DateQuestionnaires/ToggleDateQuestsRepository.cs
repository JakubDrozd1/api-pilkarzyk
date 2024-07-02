using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.DateQuest;
using DataLibrary.Model.DTO.Request.TableRequest;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.DateQuests
{
    public class ToggleDateQuestsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IToggleDateQuestsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task ToggleDateQuests(int dateQuestId, ToggleDateQuestRequest toggledateQuest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {


                var query = new QueryBuilder<USERS_DATE_QUESTS>()
                   .Select("*")
                   .From("USERS_DATE_QUESTS ")
                   .Where("IDUSER = @IdUser AND IDDATE_QUESTS = @IdQuest ");

                var UserDateQuest = await _dbConnection.QueryFirstOrDefaultAsync<USERS_DATE_QUESTS>(
                     query.Build(),
                     new { IdUser = toggledateQuest.IdUser, IdQuest = dateQuestId },
                     _fbTransaction
                 );

                var queryMeatting = new QueryBuilder<MEETINGS>()
                   .Select("ID_MEETING, DATE_QUEST_END ")
                   .From("MEETINGS ")
                   .Where("ID_MEETING = @IdMeeting ");

                var Meeting = await _dbConnection.QueryFirstOrDefaultAsync<MEETINGS>(
                     queryMeatting.Build(),
                     new { IdMeeting = toggledateQuest.IdMeeting },
                     _fbTransaction
                 );

                if (Meeting == null ||  Meeting.DATE_QUEST_END <= DateTime.Now)
                {
                    throw new Exception("Questionnaire is ended");
                }

                if ( UserDateQuest != null )
                {
                    var deleteBuilder = new QueryBuilder<USERS_DATE_QUESTS>()
                       .Delete("USERS_DATE_QUESTS ")
                       .Where("IDUSER = @IdUser AND IDDATE_QUESTS = @IdQuest ");
                    string deleteQuery = deleteBuilder.Build();
                    await _dbConnection.ExecuteAsync(
                        deleteQuery,
                        new { IdUser = toggledateQuest.IdUser, IdQuest = dateQuestId },
                         _fbTransaction
                     );
                }
                else
                {
                    var dateQuest = new { IDUSER = toggledateQuest.IdUser, IDDATE_QUESTS = dateQuestId };
                    var insertBuilder = new QueryBuilder<USERS_DATE_QUESTS>()
                    .Insert("USERS_DATE_QUESTS ", dateQuest);
                    string insertQuery = insertBuilder.Build();
                    await _dbConnection.ExecuteAsync(insertQuery, dateQuest, _fbTransaction);
                }


            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
