using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface IRankingsRepository
    {
        List<Ranking> GetAllRankings();
        Ranking? GetRankingById(int rankingId);
        void AddRanking(Ranking ranking);
        void UpdateRanking(Ranking ranking);
        void DeleteRanking(int rankingId);
    }
}
