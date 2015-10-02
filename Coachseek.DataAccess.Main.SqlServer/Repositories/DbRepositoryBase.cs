using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public abstract class DbRepositoryBase
    {
        private SqlConnection _connection;

        private string ConnectionStringKey { get; set; }

        public SqlConnection Connection
        {
            protected get
            {
                if (_connection == null)
                {
                    var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
                    _connection = new SqlConnection(connectionString);
                }

                return _connection;
            }
            set { _connection = value; }
        }


        protected DbRepositoryBase(string connectionStringKey)
        {
            ConnectionStringKey = connectionStringKey;
        }


        protected bool OpenConnection()
        {
            var wasAlreadyOpen = Connection.State == ConnectionState.Open;
            if (!wasAlreadyOpen)
                Connection.Open();

            return wasAlreadyOpen;
        }

        protected void CloseConnection(bool wasAlreadyOpen)
        {
            if (Connection != null && !wasAlreadyOpen)
                Connection.Close();
        }

        protected async Task<SqlConnection> OpenConnectionAsync()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }

        protected void CloseConnection(SqlConnection connection)
        {
            if (connection != null)
                connection.Close();
        }

        protected void CloseReader(SqlDataReader reader)
        {
            if (reader != null)
                reader.Close();
        }
    }
}
