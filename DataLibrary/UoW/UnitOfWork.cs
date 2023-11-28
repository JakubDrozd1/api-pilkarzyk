using System.Data;
using DataLibrary.ConnectionProvider;
using DataLibrary.IRepository;
using DataLibrary.Repository;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;

namespace DataLibrary.UoW
{
    public class UnitOfWork(IConnectionProvider _connectionProvider, IConfiguration configuration) : IUnitOfWork
    {
        private readonly FbConnection dbConnection = _connectionProvider.GetConnection();
        private readonly IConfiguration configuration = configuration;
        private FbTransaction? dbTransaction = null;

        public ICreateGroupsRepository CreateGroupsRepository => new CreateGroupsRepository(dbConnection);
        public ICreateMeetingsRepository CreateMeetingsRepository => new CreateMeetingsRepository(dbConnection);
        public ICreateMessagesRepository CreateMessagesRepository => new CreateMessagesRepository(dbConnection);
        public ICreateRankingsRepository CreateRankingsRepository => new CreateRankingsRepository(dbConnection);
        public ICreateUsersRepository CreateUsersRepository => new CreateUsersRepository(dbConnection);
        public ICreateGroupsUsersRepository CreateGroupsUsersRepository => new CreateGroupsUsersRepository(dbConnection);
        public ICreateTokensRepository CreateTokensRepository => new CreateTokensRepository(configuration, dbConnection);


        public IDeleteGroupsRepository DeleteGroupsRepository => new DeleteGroupsRepository(dbConnection);
        public IDeleteMeetingsRepository DeleteMeetingsRepository => new DeleteMeetingsRepository(dbConnection);
        public IDeleteMessagesRepository DeleteMessagesRepository => new DeleteMessagesRepository(dbConnection);
        public IDeleteRankingsRepository DeleteRankingsRepository => new DeleteRankingsRepository(dbConnection);
        public IDeleteUsersRepository DeleteUsersRepository => new DeleteUsersRepository(dbConnection);
        public IDeleteGroupsUsersRepository DeleteGroupsUsersRepository => new DeleteGroupsUsersRepository(dbConnection);


        public IReadGroupsRepository ReadGroupsRepository => new ReadGroupsRepository(dbConnection);
        public IReadMeetingsRepository ReadMeetingsRepository => new ReadMeetingsRepository(dbConnection);
        public IReadMessagesRepository ReadMessagesRepository => new ReadMessagesRepository(dbConnection);
        public IReadRankingsRepository ReadRankingsRepository => new ReadRankingsRepository(dbConnection);
        public IReadUsersRepository ReadUsersRepository => new ReadUsersRepository(dbConnection);
        public IReadGroupsUsersRepository ReadGroupsUsersRepository => new ReadGroupsUsersRepository(dbConnection);


        public IUpdateGroupsRepository UpdateGroupsRepository => new UpdateGroupsRepository(dbConnection);
        public IUpdateMeetingsRepository UpdateMeetingsRepository => new UpdateMeetingsRepository(dbConnection);
        public IUpdateMessagesRepository UpdateMessagesRepository => new UpdateMessagesRepository(dbConnection);
        public IUpdateRankingsRepository UpdateRankingsRepository => new UpdateRankingsRepository(dbConnection);
        public IUpdateUsersRepository UpdateUsersRepository => new UpdateUsersRepository(dbConnection);
        public IUpdateGroupsUsersRepository UpdateGroupsUsersRepository => new UpdateGroupsUsersRepository(dbConnection);

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
