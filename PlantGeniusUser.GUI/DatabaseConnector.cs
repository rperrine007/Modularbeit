using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlantGeniusUser.GUI
{
	public class DatabaseConnector
	{
		public async Task<List<Plant>> GetPlantsAsync()
		{
			// Return static data for testing
			return new List<Plant>
			{
				new Plant("Rose", DateTime.Now, 7),
				new Plant("Tulpe", DateTime.Now.AddDays(-30), 5),
				new Plant("Kaktus", DateTime.Now.AddDays(-60), 30),
			};
		}
	}
}