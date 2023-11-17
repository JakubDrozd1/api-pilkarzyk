using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using Dapper;

namespace DataLibrary.Repository
{
    public class MessagesRepository(string connectionString) : IMessagesRepository
    {
        private readonly string connectionString = connectionString;

        public List<Message> GetAllMessages()
        {
            using SqlConnection dbConnection = new(connectionString);
            dbConnection.Open();
            return dbConnection.Query<Message>("SELECT * FROM Messages").ToList();
        }

        public Message? GetMessageById(int messageId)
        {
            using SqlConnection dbConnection = new(connectionString);
            dbConnection.Open();
            return dbConnection.QueryFirstOrDefault<Message>("SELECT * FROM Messages WHERE IdMessage = @MessageId", new { MessageId = messageId });
        }

        public void AddMessage(Message message)
        {
            using SqlConnection dbConnection = new(connectionString);
            dbConnection.Open();
            dbConnection.Execute("INSERT INTO Messages (IdMeeting, IdUser, Answer) VALUES (@IdMeeting, @IdUser, @Answer)", message);
        }

        public void UpdateMessage(Message message)
        {
            using SqlConnection dbConnection = new(connectionString);
            dbConnection.Open();
            dbConnection.Execute("UPDATE Messages SET IdMeeting = @IdMeeting, IdUser = @IdUser, Answer = @Answer WHERE IdMessage = @IdMessage", message);
        }

        public void DeleteMessage(int messageId)
        {
            using SqlConnection dbConnection = new(connectionString);
            dbConnection.Open();
            dbConnection.Execute("DELETE FROM Messages WHERE IdMessage = @MessageId", new { MessageId = messageId });
        }

    }
}
