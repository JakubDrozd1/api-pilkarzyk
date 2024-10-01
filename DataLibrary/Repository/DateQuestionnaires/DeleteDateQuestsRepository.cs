using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.DateQuest;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.DateQuestionnaires
{
    public class DeleteDateQuestsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IDeleteDateQuestsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task DeleteDateQuestAsync(int dateQuestsId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var deleteBuilder = new QueryBuilder<DATE_QUESTS>()
                    .Delete("DATE_QUESTS")
                    .Where("ID_DATE_QUEST = @DateQuestsId ");
                string deleteQuery = deleteBuilder.Build();
                await _dbConnection.ExecuteAsync(deleteQuery, new { DateQuestsId = dateQuestsId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
