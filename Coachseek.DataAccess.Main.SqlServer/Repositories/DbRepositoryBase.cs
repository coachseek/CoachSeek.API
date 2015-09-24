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

        protected SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
                    _connection = new SqlConnection(connectionString);
                }

                return _connection;
            }
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

        protected async Task<bool> OpenConnectionAsync()
        {
            var wasAlreadyOpen = Connection.State == ConnectionState.Open;
            if (!wasAlreadyOpen)
                await Connection.OpenAsync();

            return wasAlreadyOpen;
        }

        protected void CloseConnection(bool wasAlreadyOpen)
        {
            if (Connection != null && !wasAlreadyOpen)
                Connection.Close();
        }
    }
}
