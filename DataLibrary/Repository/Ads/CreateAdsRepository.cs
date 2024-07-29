using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.ADS;
using DataLibrary.IRepository.GroupsUsers;
using DataLibrary.Model.DTO.Request.TableRequest;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Ads
{
    public class CreateAdsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateAdsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task AddClickToHistoryAsync(PostAdHistoryRequest postAdHistoryRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var adHistoryToAd = new
                {
                    IDUSER = postAdHistoryRequest.IDUSER,
                    IDAD = postAdHistoryRequest.IDAD,
                    DATE_CLICK = DateTime.Now
                };

                var insertBuilder = new QueryBuilder<AD_HISTORYS>().Insert("AD_HISTORYS ", adHistoryToAd);
                string insertQuery = insertBuilder.Build();
                await _dbConnection.ExecuteAsync(insertQuery, adHistoryToAd, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
