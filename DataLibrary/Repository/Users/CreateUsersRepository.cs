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
                USERS? userLogin = await readUsersRepository.GetUserByLoginAsync(user.LOGIN, localTransaction);
                USERS? userEmail = await readUsersRepository.GetUserByEmailAsync(user.EMAIL, localTransaction);
                USERS? userPhone = await readUsersRepository.GetUserByPhoneNumberAsync(user.PHONE_NUMBER, localTransaction);

                if (userLogin != null)
                {
                    throw new Exception("Acount with login already exists");
                }
                if (userEmail != null)
                {
                    throw new Exception("Acount with email already exists");
                }
                if (userPhone != null)
                {
                    throw new Exception("Acount with phone number already exists");
                }

                var insertBuilder = new QueryBuilder<USERS>()
                    .Insert("USERS ", user);
                string insertQuery = insertBuilder.Build();

                await db.ExecuteAsync(insertQuery, user, localTransaction);

                if (transaction == null)
                {
                    await localTransaction.CommitAsync();
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
