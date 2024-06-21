using MySql.Data.MySqlClient;
using SharedModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.MySql
{
    public class ReportRepository
    {
        private MySqlConnection _connection;

        public ReportRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public List<TitleId> GetAllAdReportStatuses()
        {
            string query = "SELECT ARSID, title FROM ad_report_status;";
            List<TitleId> statuses = new List<TitleId>();

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TitleId status = new TitleId
                            {
                                Id = reader.GetInt32("ARSID"),
                                Title = reader.GetString("title")
                            };
                            statuses.Add(status);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error getting ad report statuses.", ex);
                }
            }

            return statuses;
        }

        public List<TitleId> GetAllAdReportCategories()
        {
            string query = "SELECT ARCID, title FROM ad_report_category;";
            List<TitleId> categories = new List<TitleId>();

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TitleId category = new TitleId
                            {
                                Id = reader.GetInt32("ARCID"),
                                Title = reader.GetString("title")
                            };
                            categories.Add(category);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error getting ad report categories.", ex);
                }
            }

            return categories;
        }

        public void CreateCustomerAdReport(CreateCustomerReport req)
        {
            // Insert into ad_report table
            string adReportQuery = "INSERT INTO ad_report (createdAt, description, ARSID, ARCID) " +
                                   "VALUES (@createdAt, @description, @arsid, @arcid);";
            int arid;
            using (MySqlCommand adReportCommand = new MySqlCommand(adReportQuery, _connection))
            {
                adReportCommand.Parameters.AddWithValue("@createdAt", DateTime.Now);
                adReportCommand.Parameters.AddWithValue("@description", req.Description);
                adReportCommand.Parameters.AddWithValue("@arsid", req.Arsid);
                adReportCommand.Parameters.AddWithValue("@arcid", req.Arcid);

                try
                {
                    arid = (int)adReportCommand.ExecuteScalar();
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error creating ad report.", ex);
                }
            }

            // Insert into customer_ad_report table
            string customerAdReportQuery = "INSERT INTO customer_ad_report (customerID, adID, ARID) " +
                                           "VALUES (@customerID, @adID, @arid);";
            using (MySqlCommand customerAdReportCommand = new MySqlCommand(customerAdReportQuery, _connection))
            {
                customerAdReportCommand.Parameters.AddWithValue("@customerID", req.CustomerId);
                customerAdReportCommand.Parameters.AddWithValue("@adID", req.AdId);
                customerAdReportCommand.Parameters.AddWithValue("@arid", arid);

                try
                {
                    customerAdReportCommand.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error creating customer ad report.", ex);
                }
            }
        }
    }
}
