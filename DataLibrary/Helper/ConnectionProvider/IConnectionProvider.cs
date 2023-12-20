using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Helper.ConnectionProvider
{
    public interface IConnectionProvider
    {
        FbConnection GetConnection();
    }
}
