using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient; // Include the MySQL Function

namespace PlantGenius_User.GUI
{
    public class DatabaseConnector
    {
        public bool ConnectToDatabase()
        {
            string connectionString = "Server=s91z78.meinserver.io;Database=c1zhaw;User=c1_zhaw;Password=#sly3rC1TB;";
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connected to MariaDB successfully.");
                    return true; // Connection was successful
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to connect to MariaDB. Error: {ex.Message}");
                    return false; // Connection failed
                }
            }
        }
    }

}
