using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Users;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Ads
{

    public class ReadAdsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadAdsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        private string SELECT = "* ";

        public async Task<GetAdResponse?> GetAdsAsync()
        {

            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                DynamicParameters dynamicParameters = new();


                var query = new QueryBuilder<ADS>()
                    .Select(SELECT)
                    .From("ADS a")
                    .Where($"a.{nameof(ADS.DATE_START)} <= @Date and a.{nameof(ADS.DATE_END)} >= @Date ");

                dynamicParameters.Add("@Date", DateTime.Now);


                var ads = (await _dbConnection.QueryAsync<ADS>(query.Build(), dynamicParameters, _fbTransaction)).AsList();


                if (ads != null && ads.Count > 0)
                {
                    int r = ads.FindIndex(ad => ad.TIME_START <= DateTime.Now.Minute && ad.TIME_END >= DateTime.Now.Minute);

                    if (r == -1)
                    {
                        Random rnd = new Random();
                        r = rnd.Next(ads.Count);
                    }

                    return new GetAdResponse { Id = (int)ads[r].ID_AD, Content = ads[r].CONTENT!, Url = ads[r].URL ?? "", Color = ads[r].COLOR ?? "" };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }


        public async Task<List<GetAddWithClicksResponse>> GetAllAdsAsync()
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<ADS>()
                    .Select($"ADS.{nameof(ADS.ID_AD)} AS IdAd, " +
                    $"ADS.{nameof(ADS.DATE_START)}, " +
                    $"ADS.{nameof(ADS.DATE_END)}, " +
                    $"ADS.{nameof(ADS.CONTENT)}, " +
                    $"ADS.{nameof(ADS.URL)}, " +
                    $"ADS.{nameof(ADS.COLOR)}, " +
                    $"ADS.{nameof(ADS.TIME_START)}, " +
                    $"ADS.{nameof(ADS.TIME_END)}, " +
                    $"COUNT(AD_HISTORYS.{nameof(AD_HISTORYS.ID_AD_HISTORY)}) AS ClickCount ")
                    .From($"{nameof(ADS)} " +
                    $"LEFT JOIN {nameof(AD_HISTORYS)} ON ADS.{nameof(ADS.ID_AD)} = AD_HISTORYS.{nameof(AD_HISTORYS.IDAD)} ")
                    .GroupBy($"ADS.{nameof(ADS.ID_AD)}, " +
                    $"ADS.{nameof(ADS.DATE_START)}, " +
                    $"ADS.{nameof(ADS.DATE_END)}, " +
                    $"ADS.{nameof(ADS.CONTENT)}, " +
                    $"ADS.{nameof(ADS.URL)}, " +
                    $"ADS.{nameof(ADS.COLOR)}, " +
                    $"ADS.{nameof(ADS.TIME_START)}, " +
                    $"ADS.{nameof(ADS.TIME_END)} ");


                return (await _dbConnection.QueryAsync<GetAddWithClicksResponse>(query.Build())).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

    }
}
