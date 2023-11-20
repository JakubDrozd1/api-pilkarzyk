using System.Data;
using DataLibrary.Repository;
using DataLibrary.ConnectionProvider;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.IRepository.Users;
using DataLibrary.IRepository.Groups;
using DataLibrary.IRepository.Messages;
using DataLibrary.IRepository.Rankings;
using DataLibrary.IRepository.Meetings;
using DataLibrary.IRepository;
using DataLibrary.Repository.Messages;
using DataLibrary.Repository.Rankings;
using DataLibrary.Repository.Users;

namespace DataLibrary.UoW
{
    public class UnitOfWork(IConnectionProvider _connectionProvider) : IUnitOfWork
    {
        private readonly FbConnection dbConnection = _connectionProvider.GetConnection();
        private FbTransaction? dbTransaction = null;

        public ICreateGroupsRepository CreateGroupsRepository => new CreateGroupsRepository(dbConnection);
        public ICreateMeetingsRepository CreateMeetingsRepository => new CreateMeetingsRepository(dbConnection);
        public ICreateMessagesRepository CreateMessagesRepository => new CreateMessagesRepository(dbConnection);
        public ICreateRankingsRepository CreateRankingsRepository => new CreateRankingsRepository(dbConnection);
        public ICreateUsersRepository CreateUsersRepository => new CreateUsersRepository(dbConnection);

        public IDeleteGroupsRepository DeleteGroupsRepository => new DeleteGroupsRepository(dbConnection);
        public IDeleteMeetingsRepository DeleteMeetingsRepository => new DeleteMeetingsRepository(dbConnection);
        public IDeleteMessagesRepository DeleteMessagesRepository => new DeleteMessagesRepository(dbConnection);
        public IDeleteRankingsRepository DeleteRankingsRepository => new DeleteRankingsRepository(dbConnection);
        public IDeleteUsersRepository DeleteUsersRepository => new DeleteUsersRepository(dbConnection);

        public IReadGroupsRepository ReadGroupsRepository => new ReadGroupsRepository(dbConnection);
        public IReadMeetingsRepository ReadMeetingsRepository => new ReadMeetingsRepository(dbConnection);
        public IReadMessagesRepository ReadMessagesRepository => new ReadMessagesRepository(dbConnection);
        public IReadRankingsRepository ReadRankingsRepository => new ReadRankingsRepository(dbConnection);
        public IReadUsersRepository ReadUsersRepository => new ReadUsersRepository(dbConnection);

        public IUpdateGroupsRepository UpdateGroupsRepository => new UpdateGroupsRepository(dbConnection);
        public IUpdateMeetingsRepository UpdateMeetingsRepository => new UpdateMeetingsRepository(dbConnection);
        public IUpdateMessagesRepository UpdateMessagesRepository => new UpdateMessagesRepository(dbConnection);
        public IUpdateRankingsRepository UpdateRankingsRepository => new UpdateRankingsRepository(dbConnection);
        public IUpdateUsersRepository UpdateUsersRepository => new UpdateUsersRepository(dbConnection);

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
