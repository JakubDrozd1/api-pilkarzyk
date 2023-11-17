using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.IRepository;
using DataLibrary.Repository;
using Microsoft.Extensions.Configuration;
using DataLibrary.ConnectionProvider;

namespace DataLibrary.UoW
{
    public class UnitOfWork(IConnectionProvider _connectionProvider) : IUnitOfWork
    {
        private readonly SqlConnection dbConnection = _connectionProvider.GetConnection();
        private SqlTransaction? dbTransaction = null;

        public IUsersRepository UsersRepository => new UsersRepository(dbConnection);

        public IGroupsRepository GroupsRepository => throw new NotImplementedException();

        public IMeetingsRepository MeetingsRepository => throw new NotImplementedException();

        public IMessagesRepository MessagesRepository => throw new NotImplementedException();

        public IRankingsRepository RankingsRepository => throw new NotImplementedException();

        //public IGroupsRepository GroupsRepository => new GroupsRepository(dbConnection, dbTransaction);
        //public IMeetingsRepository MeetingsRepository => new MeetingsRepository(dbConnection, dbTransaction);
        //public IMessagesRepository MessagesRepository => new MessagesRepository(dbConnection, dbTransaction);
        //public IRankingsRepository RankingsRepository => new RankingsRepository(dbConnection, dbTransaction);

        public async Task SaveChangesAsync()
        {
            if (dbTransaction != null)
            {
                await dbTransaction.CommitAsync();
                dbTransaction = null;
            }
        }

        public void Dispose()
        {
            dbTransaction?.Rollback();
            dbTransaction = null;

            if (dbConnection != null && dbConnection.State == ConnectionState.Open)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public void BeginTransaction()
        {
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();

            dbTransaction = dbConnection.BeginTransaction();
        }
    }
}
