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

			try
			{
				using (var connection = new MySqlConnection(connectionString))
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
			}
			catch (TypeInitializationException ex)
			{
				Console.WriteLine($"TypeInitializationException: {ex.Message}");
				if (ex.InnerException != null)
				{
					Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception: {ex.Message}");
			}

			return plants;
		}
		//public async Task<List<Plant>> GetPlantsAsync()
		//{
		//    // Return static data for testing
		//    return new List<Plant>
		//    {
		//        new Plant("Rose", DateTime.Now, 7),
		//        new Plant("Tulpe", DateTime.Now.AddDays(-30), 5),
		//        new Plant("Kaktus", DateTime.Now.AddDays(-60), 30),
		//    };
		//}
	}
}