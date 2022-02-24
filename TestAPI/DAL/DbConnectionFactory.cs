using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace TestAPI.DAL
{
    public class DbConnectionFactory
    {
        private IConfiguration Configuration;

        public DbConnectionFactory(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        public MySqlConnection MySqlConnection()
        {
            return new MySqlConnection(Configuration.GetConnectionString("DbConnection"));
        }
        
    }
}