using System.Data;
using DataLibrary.Helper.ConnectionProvider;
using DataLibrary.IRepository.EmailSender;
using DataLibrary.IRepository.Groups;
using DataLibrary.IRepository.GroupsUsers;
using DataLibrary.IRepository.Meetings;
using DataLibrary.IRepository.Messages;
using DataLibrary.IRepository.Rankings;
using DataLibrary.IRepository.Tokens;
using DataLibrary.IRepository.Users;
using DataLibrary.IRepository.UsersMeetings;
using DataLibrary.Repository.EmailSender;
using DataLibrary.Repository.Groups;
using DataLibrary.Repository.GroupsUsers;
using DataLibrary.Repository.Meetings;
using DataLibrary.Repository.Messages;
using DataLibrary.Repository.Rankings;
using DataLibrary.Repository.Tokens;
using DataLibrary.Repository.Users;
using DataLibrary.Repository.UsersMeetings;
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
        public ICreateUsersMeetingsRepository CreateUsersMeetingRepository => new CreateUsersMeetingsRepository(dbConnection);



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
        public IReadEmailSender ReadEmailSender => new ReadEmailSenderRepository(dbConnection);
        public IReadUsersMeetingsRepository ReadUsersMeetingsRepository => new ReadUsersMeetingsRepository(dbConnection);



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
