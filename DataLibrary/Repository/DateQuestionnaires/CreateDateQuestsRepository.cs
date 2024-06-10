using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.DateQuest;
using DataLibrary.Model.DTO.Request.TableRequest;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.DateQuests
{
    public class CreateDateQuestsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateDateQuestsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task AddDateQuestAsync(GetDateQuestRequest dateQuest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var insertBuilder = new QueryBuilder<DATE_QUESTS>()
                    .Insert("DATE_QUESTS ", dateQuest);
                string insertQuery = insertBuilder.Build();
                await _dbConnection.ExecuteAsync(insertQuery, dateQuest, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}"); ;
            }
        }
    }
}
