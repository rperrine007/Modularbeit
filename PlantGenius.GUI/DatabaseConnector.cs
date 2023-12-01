using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PlantGenius.GUI
{
    public class DatabaseConnector
    {
        public async Task<MySqlConnection> GetDatabaseConnectionAsync()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "49.12.196.20", // Server
                Port = 14501,            // Port number
                Database = "c1_zhaw2",     // Database
                UserID = "c1_zhaw",      // User
                Password = "lQ9fKVoNK7ll!" // Password
            };

            var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();
            return connection;
        }
    }

}
