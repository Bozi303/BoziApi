using Infrastructure.DataAccess.MySql.MySqlModels;
using MySql.Data.MySqlClient;
using SharedModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.MySql
{
    public class CityRepository
    {
        private MySqlConnection _connection;

        public CityRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public List<MySqlProvince> GetAllProvinces()
        {
            List<MySqlProvince> provinces = new List<MySqlProvince>();

            string query = "SELECT * FROM province";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MySqlProvince province = new MySqlProvince
                            {
                                ProvinceId = reader.GetInt32("ProvinceID"),
                                ProvinceName = reader.GetString("Name")
                            };

                            provinces.Add(province);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Error retrieving provinces.", ex);
                }
            }

            return provinces;
        }

        public List<MySqlCity> GetCitiesByProvinceId(string provinceId)
        {
            List<MySqlCity> cities = new List<MySqlCity>();

            string query = "SELECT * FROM city WHERE ProvinceID = @provinceId";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@provinceId", provinceId);

                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MySqlCity city = new MySqlCity
                            {
                                CityId = reader.GetInt32("CityID"),
                                CityName = reader.GetString("Name"),
                            };

                            cities.Add(city);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Error retrieving cities by provinceId.", ex);
                }
            }

            return cities;
        }
    }
}
