using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using PlantGenius.DAL.Models;

namespace PlantGenius.DAL
{
    public class DataAccessLayer_OLD
    {

        /// <summary>
        /// In this asynchronous task a query to get the room data is made to the DB.
        /// Why asynchronous: This ensures that the application remains responsive and can handle
        /// other tasks while waiting for the database to return results.
        ///  </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public async static Task GetRooms(DatabaseConnector_OLD dbConnectorInput, Collection<Room> roomListInput)
        {
            roomListInput.Clear();
            // Use the 'GetDatabaseConnectionAsync' method to asynchronously obtain a database connection.
            // The 'await' keyword is used to await the completion of the asynchronous operation.
            using (var connection = await dbConnectorInput.GetDatabaseConnectionAsync())
            {
                // Sort by set RoomSort.
                string query = "SELECT * FROM Room ORDER BY RoomSort ASC";

                // the defined query is made on the defined connection (DB)
                using (var command = new MySqlCommand(query, connection))
                {
                    //the data is asynchroned read from the DB. 
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            //Add Rooms of DB to roomList
                            roomListInput.Add(new Room()
                            {
                                RoomID = (int)reader["RoomID"],
                                RoomName = (string)reader["RoomName"],
                                RoomSort = (int)reader["RoomSort"],
                                RoomFloor = (int)reader["RoomFloor"],
                                RoomLight = (bool)reader["RoomLight"]
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// add a room to the DB.
        /// </summary>
        /// <param name="dbConnectorInput"></param>
        /// <param name="roomInput"></param>
        public async static Task AddRoomToDB(DatabaseConnector_OLD dbConnectorInput, Room roomInput)
        {
            // Insert into database
            using (var connection = await dbConnectorInput.GetDatabaseConnectionAsync())
            {
                string query = $"INSERT INTO Room (RoomName, RoomSort, RoomFloor, RoomLight) VALUES ('{roomInput.RoomName}', {roomInput.RoomSort}, {roomInput.RoomFloor}, {roomInput.RoomLight})";
                using (var command = new MySqlCommand(query, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Delete room from DB. 
        /// </summary>
        /// <param name="dbConnectorInput"></param>
        /// <param name="roomInput"></param>
        /// <returns></returns>
        public async static Task DeleteRoomFromDB(DatabaseConnector_OLD dbConnectorInput, Collection<Room> roomListInput, Room roomInput)
        {
            //delete the room from the database
            string query = $"DELETE FROM Room WHERE RoomID = {roomInput.RoomID}";

            using (var connection = await dbConnectorInput.GetDatabaseConnectionAsync())
            {
                using (var command = new MySqlCommand(query, connection))
                {

                    await command.ExecuteNonQueryAsync();
                    //Call resorting method
                    await DataAccessLayer_OLD.OnRoomDeleteNewSort(dbConnectorInput, roomListInput);
                }
            }
            await GetRooms(dbConnectorInput, roomListInput);
        }

        /// <summary>
        /// Update the SortNumber of the rooms, when a room is deleted.
        /// </summary>
        /// <returns></returns>
        public async static Task OnRoomDeleteNewSort(DatabaseConnector_OLD dbConnectorInput, Collection<Room> roomListInput)
        {
            // Sort the Rooms by RoomSortNumber
            var sortedRooms = roomListInput.OrderBy(room => room.RoomSort).ToList();

            // Adding a new sorting number to each room to avoid gaps
            int newSortID = 1;
            foreach (var room in sortedRooms)
            {
                // Update Database
                using (var connection = await dbConnectorInput.GetDatabaseConnectionAsync())
                {
                    string query = $"UPDATE Room SET RoomSort = {newSortID} WHERE RoomID = {room.RoomID}";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
                newSortID++;
            }
        }

        /// <summary>
        /// In this asynchronous task a query to get the room data is made to the DB.
        /// Why asynchronous: This ensures that the application remains responsive and can handle other tasks while waiting for the database to return results.
        ///  </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public async static Task UpdateDBChangeRoomSortNumber(MySqlConnection connection, int roomIDSelected, int roomSortChanged)
        {
            // query to update the roomSortNumber of a specific room
            string query = $"UPDATE `Room` SET `Room`.`RoomSort` = '{roomSortChanged}' WHERE `Room`.`RoomID` = {roomIDSelected}";

            // Using MySqlCommand to execute the query on the specified connection
            using (var command = new MySqlCommand(query, connection))
            {
                // Executing the query asynchronously
                await command.ExecuteNonQueryAsync();
            }
        }

    }
}
