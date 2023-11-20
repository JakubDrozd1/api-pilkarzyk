using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.ConnectionProvider
{
    public interface IConnectionProvider
    {
        FbConnection GetConnection();
    }
}
