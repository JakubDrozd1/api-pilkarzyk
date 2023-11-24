using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class CreateUsersRepository(FbConnection dbConnection) : ICreateUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task AddUserAsync(USERS user, FbTransaction? transaction = null)
        {
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            FbTransaction localTransaction = transaction ?? await db.BeginTransactionAsync();

            try
            {
                ReadUsersRepository readUsersRepository = new(db);
                USERS? userTmp = await readUsersRepository.GetUserByLoginAsync(user.LOGIN, localTransaction);

                if (userTmp != null)
                {
                    throw new Exception("Login already exists");
                }

                var insertBuilder = new QueryBuilder<USERS>()
                    .Insert("USERS ", user);
                string insertQuery = insertBuilder.Build();

                await db.ExecuteAsync(insertQuery, user, localTransaction);

                if (transaction == null)
                {
                    localTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                localTransaction?.Rollback();
                throw new Exception($"Error while executing query: {ex.Message}");
            }
        }
    }
}
