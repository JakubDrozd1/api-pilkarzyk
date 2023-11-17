using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.ConnectionProvider
{
    public interface IConnectionProvider
    {
        SqlConnection GetConnection();
    }
}
