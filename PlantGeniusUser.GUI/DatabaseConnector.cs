using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlantGeniusUser.GUI
{
	public class DatabaseConnector
	{
		private string connectionString = "server=49.12.196.20;port=14501;user=c1_zhaw;password=lQ9fKVoNK7ll!;database=c1_zhaw2";

		public async Task<List<Plant>> GetPlantsAsync()
		{

			var plants = new List<Plant>();

			//var builder = new MySqlConnectionStringBuilder
			//{
			//	Server = "49.12.196.20",		// Server
			//	Port = 14501,					// Port number
			//	Database = "c1_zhaw2",			// Database
			//	UserID = "c1_zhaw",				// User
			//	Password = "lQ9fKVoNK7ll!"		// Password
			//};
			using (var connection = new MySqlConnection(connectionString))
			//using (var connection = new MySqlConnection(builder.ConnectionString))
			{
				await connection.OpenAsync();

				using (var command = new MySqlCommand("SELECT * FROM Plant", connection))
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						var plant = new Plant(
							reader["PlantName"].ToString(),
							Convert.ToDateTime(reader["PlantWaterLastTime"]),
							Convert.ToInt32(reader["PlantWaterRequirement"])
						);
						plants.Add(plant);
					}
				}
			}

			return plants;


			//// Return static data for testing
			//return new List<Plant>
			//{
			//    new Plant("Rose", DateTime.Now, 7),
			//    new Plant("Tulpe", DateTime.Now.AddDays(-30), 5),
			//    new Plant("Kaktus", DateTime.Now.AddDays(-60), 30),
			//};
		}
	}
}