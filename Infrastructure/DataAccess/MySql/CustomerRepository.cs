using Infrastructure.DataAccess.MySql.MySqlModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.MySql
{
    public class CustomerRepository
    {
        private readonly MySqlConnection _connection;

        public CustomerRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public MySqlCustomer? GetCustomerByMobileNumberAsync(string mobileNumber)
        {
            MySqlCustomer? customer = null;

            string query = "SELECT * FROM customer WHERE mobileNumber = @mobileNumber";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@mobileNumber", mobileNumber);

                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customer = new MySqlCustomer
                            {
                                CustomerID = reader.GetInt32("customerID"),
                                MobileNumber = reader.GetString("mobileNumber"),
                                Email = reader.GetString("email"),
                                FirstName = reader.GetString("firstname"),
                                LastName = reader.GetString("lastname"),
                                LastAccess = reader.GetDateTime("lastaccess"),
                                RegisterTime = reader.GetDateTime("registerTime"),
                                CSID = reader.GetInt32("CSID"),
                                CityID = reader.GetInt32("CityID")
                            };
                        }
                    }
                }


                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error retrieving customer by mobile number.", ex);
                }
            }
                
            return customer;
        }
    }
}
