using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient; // Include the MySQL Function

namespace PlantGenius_User.GUI
{
    public class DatabaseConnector
    {
        public bool ConnectToDatabase()
        {
            var builder = new MySqlConnectionStringBuilder()
            {
                Server = "49.12.196.20",         // Server
                Port = 14500,                   // Port number
                Database = "c1zhaw",           // Database
                UserID = "c1_zhaw",           // User
                Password = "lQ9fKVoNK7ll!"   // Password
            };

            string connectionString = builder.ConnectionString;

            // SQL-Abfrage, um alle Felder aus der Tabelle "Room" abzurufen
            string query = "SELECT * FROM Room";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            // Über die Ergebnisse iterieren
                            while (reader.Read())
                            {
                                // Beispiel: Ausgabe der Werte für jedes Feld
                                MessageBox.Show($"RoomId: {reader["RoomId"]}, RoomName: {reader["RoomName"]}, ...");
                                // Fügen Sie hier die Ausgabe für alle Felder hinzu
                            }
                        }

                    }
                    MessageBox.Show("Connected to MariaDB successfully.");
                    return true; // Connection was successful
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to connect to MariaDB. Error: {ex.Message}");
                    return false; // Connection failed
                }
            }
        }
    }

}
