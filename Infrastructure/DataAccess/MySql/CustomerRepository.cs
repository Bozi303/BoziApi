﻿using Infrastructure.DataAccess.MySql.MySqlModels;
using Infrastructure.Model;
using MySql.Data.MySqlClient;
using SharedModel.Models;
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

        public MySqlCustomer? GetCustomerByMobileNumber(string mobileNumber)
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
                                CustomerID = reader.GetInt32("customerID").ToString(),
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
        
        public MySqlCustomer? GetCustomerById (string customerId)
        {
            MySqlCustomer? customer = null;

            string query = "SELECT * FROM customer WHERE customerID = @customerId";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@customerId", customerId);

                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customer = new MySqlCustomer
                            {
                                CustomerID = reader.GetInt32("customerID").ToString(),
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
                    throw new Exception("Error retrieving customer by customerId.", ex);
                }
            }

            return customer;
        }

        public int CreateCustomer(CreateCustomerRequest customer)
        {
            string query = @"
        INSERT INTO customer (mobileNumber, email, firstname, lastname, lastaccess, registerTime, CSID, CityID)
        VALUES (@mobileNumber, @email, @firstname, @lastname, @lastaccess, @registerTime, @CSID, @CityID)";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@mobileNumber", customer.MobileNumber);
                command.Parameters.AddWithValue("@email", customer.Email);
                command.Parameters.AddWithValue("@firstname", customer.FirstName);
                command.Parameters.AddWithValue("@lastname", customer.LastName);
                command.Parameters.AddWithValue("@lastaccess", customer.LastAccess);
                command.Parameters.AddWithValue("@registerTime", customer.RegisterationDate);
                command.Parameters.AddWithValue("@CSID", customer.CustomerStatus);
                command.Parameters.AddWithValue("@CityID", customer.CityId);

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Get the newly inserted customerID
                        command.CommandText = "SELECT LAST_INSERT_ID()";
                        object result = command.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error creating customer.", ex);
                }
            }
        }

        public int CreateAd(CreateAdRequest ad)
        {
            string query = @"
        INSERT INTO ad (title, description, price, createdAt, updatedAt, customerID, storeID, cityID, statusID, ACID)
        VALUES (@title, @description, @price, @createdAt, @updatedAt, @customerID, @storeID, @cityID, @statusID, @ACID)";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@title", ad.Title);
                command.Parameters.AddWithValue("@description", ad.Description);
                command.Parameters.AddWithValue("@price", ad.Price);
                command.Parameters.AddWithValue("@createdAt", ad.CreatedAt);
                command.Parameters.AddWithValue("@updatedAt", ad.UpdatedAt);
                command.Parameters.AddWithValue("@customerID", ad.CustomerId ?? "-1"); // Use null-coalescing operator to handle null values
                command.Parameters.AddWithValue("@storeID", ad.StoreId ?? "-1");
                command.Parameters.AddWithValue("@cityID", ad.CityId);
                command.Parameters.AddWithValue("@statusID", ad.StatusId);
                command.Parameters.AddWithValue("@ACID", ad.AdCategoryId);

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Get the newly inserted adID
                        command.CommandText = "SELECT LAST_INSERT_ID()";
                        object result = command.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error creating ad.", ex);
                }
            }
        }
    }
}
