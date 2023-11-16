using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;

namespace DataLibrary.Repository
{
    internal class RankingsRepository : IRankingsRepository
    {
        private readonly string connectionString;

        public RankingsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Ranking> GetAllRankings()
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                return dbConnection.Query<Ranking>("SELECT * FROM Rankings").ToList();
            }
        }

        public Ranking? GetRankingById(int rankingId)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                return dbConnection.QueryFirstOrDefault<Ranking>("SELECT * FROM Rankings WHERE IdRanking = @RankingId", new { RankingId = rankingId });
            }
        }

        public void AddRanking(Ranking ranking)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("INSERT INTO Rankings (DateMeeting, IdUser, IdGroup, Point) VALUES (@DateMeeting, @IdUser, @IdGroup, @Point)", ranking);
            }
        }

        public void UpdateRanking(Ranking ranking)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("UPDATE Rankings SET DateMeeting = @DateMeeting, IdUser = @IdUser, IdGroup = @IdGroup, Point = @Point WHERE IdRanking = @IdRanking", ranking);
            }
        }

        public void DeleteRanking(int rankingId)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("DELETE FROM Rankings WHERE IdRanking = @RankingId", new { RankingId = rankingId });
            }
        }
    }
}
