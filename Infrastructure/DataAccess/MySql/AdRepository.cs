using Infrastructure.DataAccess.MySql.MySqlModels;
using MySql.Data.MySqlClient;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.MySql
{
    public class AdRepository
    {
        private readonly MySqlConnection _connection;

        public AdRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public List<MySqlAd>? GetAdRepositoriesByCustomerId(string customerId)
        {
            List<MySqlAd>? adRepositories = new();

            string query = "SELECT * FROM ad WHERE custromerId = @customerId";

            using MySqlCommand command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("customerId", customerId);

            try
            {
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var ad = MapMySqlAd(reader);
                    adRepositories.Add(ad);
                }
                return adRepositories;
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error retrieving ad by customerId.", ex);
            }
        }

        public MySqlAd MapMySqlAd(MySqlDataReader reader)
        {
            return new MySqlAd
            {
                ACID = reader.GetInt32("ACID"),
                AdId = reader.GetInt32("adID"),
                CityId = reader.GetInt32("cityID"),
                CreatedAt = reader.GetDateTime("createdAt"),
                CustomerId = reader.GetInt32("customerID"),
                Description = reader.GetString("description"),
                Price = reader.GetDecimal("price"),
                StatusId = reader.GetInt32("statusID"),
                StoreId = reader.GetInt32("storeID"),
                Title = reader.GetString("title"),
                UpdatedAt = reader.GetDateTime("updatedAt")
            };
        }
    }
}
