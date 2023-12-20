using Microsoft.Extensions.Configuration;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Helper.ConnectionProvider
{
    public class ConnectionProvider(IConfiguration configuration) : IConnectionProvider
    {
        private readonly IConfiguration _configuration = configuration;

        public FbConnection GetConnection()
        {
            return new FbConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
