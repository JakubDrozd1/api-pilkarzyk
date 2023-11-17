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
    public class MeetingsRepository(string connectionString) : IMeetingsRepository
    {
        private readonly string connectionString = connectionString;

        public List<Meeting> GetAllMeetings()
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();
            return dbConnection.Query<Meeting>("SELECT * FROM Meetings").ToList();
        }

        public Meeting? GetMeetingById(int meetingId)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();
            return dbConnection.QueryFirstOrDefault<Meeting>("SELECT * FROM Meetings WHERE IdMeeting = @MeetingId", new { MeetingId = meetingId });
        }

        public void AddMeeting(Meeting meeting)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();
            dbConnection.Execute("INSERT INTO Meetings (DateMeeting, Place, Quantity, Description, IdUser, IdGroup) VALUES (@DateMeeting, @Place, @Quantity, @Description, @IdUser, @IdGroup)", meeting);
        }

        public void UpdateMeeting(Meeting meeting)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();
            dbConnection.Execute("UPDATE Meetings SET DateMeeting = @DateMeeting, Place = @Place, Quantity = @Quantity, Description = @Description, IdUser = @IdUser, IdGroup = @IdGroup WHERE IdMeeting = @IdMeeting", meeting);
        }

        public void DeleteMeeting(int meetingId)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();
            dbConnection.Execute("DELETE FROM Meetings WHERE IdMeeting = @MeetingId", new { MeetingId = meetingId });
        }
    }
}
