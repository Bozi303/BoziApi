using MySql.Data.MySqlClient;
using SharedModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.MySql
{
    public class StoreRepository
    {
        private readonly MySqlConnection _connection;

        public StoreRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public List<TitleId> GetStoreCategories()
        {
            List<TitleId> storeCategories = new List<TitleId>();

            string query = "SELECT SCID, Title FROM store_category;";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TitleId storeCategory = new TitleId
                            {
                                Id = reader.GetInt32("SCID"),
                                Title = reader.GetString("Title")
                            };

                            storeCategories.Add(storeCategory);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error getting store categories.", ex);
                }
            }

            return storeCategories;
        }

        public void InsertStore(CreateStore store)
        {
            string query = @"
                INSERT INTO store (
                    name, description, registerNumber, customerID, SCID, statusID, CityID
                )
                VALUES (
                    @name, @description, @registerNumber, @customerID, @SCID, @statusID, @CityID
                );
            ";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@name", store.Name);
                command.Parameters.AddWithValue("@description", store.Description);
                command.Parameters.AddWithValue("@registerNumber", store.RegisterNumber);
                command.Parameters.AddWithValue("@customerID", store.CustomerID);
                command.Parameters.AddWithValue("@SCID", store.CategoryId);
                command.Parameters.AddWithValue("@statusID", store.StatusId);
                command.Parameters.AddWithValue("@CityID", store.CityId);

                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Store inserted successfully.");
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error inserting store.", ex);
                }
            }
        }

        public bool IsCustomerStoreOwner(string storeID, string customerID)
        {
            string query = @"
                SELECT s.storeID, s.customerID
                FROM store s
                WHERE s.storeID = @storeID AND s.customerID = @customerID
            ";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@storeID", storeID);
                command.Parameters.AddWithValue("@customerID", customerID);

                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error checking customer store ownership.", ex);
                }
            }
        }

        public void InsertSupportStoreStatus(string note, string supportID, string storeID, string statusID, DateTime createdAt)
        {
            string query = @"
                INSERT INTO support_store_status (
                    note, supportID, storeID, statusID, createdAt
                ) VALUES (
                    @note, @supportID, @storeID, @statusID, @createdAt
                );
            ";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@note", note);
                command.Parameters.AddWithValue("@supportID", supportID);
                command.Parameters.AddWithValue("@storeID", storeID);
                command.Parameters.AddWithValue("@statusID", statusID);
                command.Parameters.AddWithValue("@createdAt", createdAt);

                try
                {
                    _connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("New support store status record inserted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to insert new support store status record.");
                    }
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error inserting new support store status record.", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public void UpdateStoreStatus(string storeID, string newStatusID)
        {
            string query = @"
                UPDATE store
                SET statusID = @newStatusID
                WHERE storeID = @storeID;
            ";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@newStatusID", newStatusID);
                command.Parameters.AddWithValue("@storeID", storeID);

                try
                {
                    _connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Store status updated for store ID {storeID}.");
                    }
                    else
                    {
                        Console.WriteLine($"No store found with ID {storeID}.");
                    }
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception($"Error updating store status for store ID {storeID}.", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public List<TitleId> GetStoreStatuses()
        {
            List<TitleId> storeStatuses = new List<TitleId>();

            string query = @"
                SELECT statusID, Title
                FROM store_status;
            ";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                try
                {
                    _connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TitleId storeStatus = new TitleId
                            {
                                Id = reader.GetInt32("statusID"),
                                Title = reader.GetString("Title")
                            };
                            storeStatuses.Add(storeStatus);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error retrieving store statuses.", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }

            return storeStatuses;
        }
    }
}
