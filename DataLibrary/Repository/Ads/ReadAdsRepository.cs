using System.Collections.Generic;
using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Users;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;
using Xamarin.Essentials;

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


                var ads =  (await _dbConnection.QueryAsync<ADS>(query.Build(), dynamicParameters, _fbTransaction)).AsList();


                if(ads != null && ads.Count > 0)
                {
                    Random rnd = new Random();
                    int r = rnd.Next(ads.Count);
                    return new GetAdResponse { Content = ads[r].CONTENT!, Url = ads[r].URL ?? "" };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

    }
}
