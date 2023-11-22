using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface ICreateRankingsRepository
    { 
        Task AddRankingAsync(Ranking ranking, FbTransaction? transaction = null); 
    }
}
