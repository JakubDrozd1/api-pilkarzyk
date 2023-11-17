using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DataLibrary.ConnectionProvider
{
    public class ConnectionProvider(IConfiguration configuration) : IConnectionProvider
    {
        private readonly IConfiguration _configuration = configuration;

        public SqlConnection GetConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
