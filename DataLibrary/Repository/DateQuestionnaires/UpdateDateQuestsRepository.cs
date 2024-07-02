using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.DateQuest;
using DataLibrary.IRepository.Groups;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.DateQuests
{
    public class UpdateDateQuestsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IUpdateDateQuestsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task UpdateDateQuestAsync(DATE_QUESTS dateQuest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var updateBuilder = new QueryBuilder<DATE_QUESTS>()
                    .Update("DATE_QUESTS ", dateQuest)
                    .Where("ID_DATE_QUEST = @ID_DATE_QUEST ");
                string updateQuery = updateBuilder.Build();
                await _dbConnection.ExecuteAsync(updateQuery, dateQuest, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
