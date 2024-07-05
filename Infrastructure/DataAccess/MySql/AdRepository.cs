using Infrastructure.DataAccess.MySql.MySqlModels;
using MySql.Data.MySqlClient;
using SharedModel.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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

        public void CreateAdMeta(int adID, string name, string value)
        {
            string query = @"
        INSERT INTO ad_meta (adID, name, value)
        VALUES (@adID, @name, @value)";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@adID", adID);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@value", value);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error creating ad meta.", ex);
                }
            }
        }

        public void CreateAdView(int adID, int viewCount)
        {
            string query = @"
        INSERT INTO ad_views (adID, viewCount)
        VALUES (@adID, @viewCount)";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@adID", adID);
                command.Parameters.AddWithValue("@viewCount", viewCount);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error creating ad view.", ex);
                }
            }
        }

        public void CreateAdPicture(int adID, string pictureURL)
        {
            string query = @"
                INSERT INTO ad_pictures (adID, pictureURL)
                VALUES (@adID, @pictureURL)";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@adID", adID);
                command.Parameters.AddWithValue("@pictureURL", pictureURL);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error creating ad picture.", ex);
                }
            }
        }


        public List<string> GetMetaKeysByCategoryId(string categoryId)
        {
            List<string> metaKeys = new List<string>();

            string query = "SELECT metaKey FROM category_metadata WHERE ACID = @categoryId";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@categoryId", categoryId);

                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            metaKeys.Add(reader.GetString("metaKey"));
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Error retrieving metadata keys by category ID.", ex);
                }
            }

            return metaKeys;
        }

        public List<MySqlAdCategory> GetCategoriesByParentId(string parentId)
        {
            List<MySqlAdCategory> categories = new List<MySqlAdCategory>();

            string query = "SELECT ACID, parentId, title, isLeaf FROM ad_category WHERE parentId = @parentId";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@parentId", parentId);

                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var category = new MySqlAdCategory
                            {
                                ACID = reader.GetInt32("ACID"),
                                ParentId = reader.GetInt32("parentId").ToString(),
                                Title = reader.GetString("title"),
                                IsLeaf = reader.GetBoolean("isLeaf")
                            };
                            categories.Add(category);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Error retrieving categories by parent ID.", ex);
                }
            }

            return categories;
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


        public List<MySqlAdPreview> GetAdPreviews(GetAdPreview filter)
        {
            List<MySqlAdPreview> adPreviews = new List<MySqlAdPreview>();

            string query = @"
        SELECT
            a.adID,
            a.title,
            a.createdAt,
            a.price,
            (SELECT pictureURL
             FROM ad_pictures
             WHERE adID = a.adID
             ORDER BY APID
             LIMIT 1) AS picture
        FROM ad a
        JOIN city c ON a.cityID = c.cityID
        JOIN ad_category ac ON a.ACID = ac.ACID
        JOIN status s ON a.statusID = s.statusID
        WHERE
            a.title LIKE CONCAT('%', @search, '%')
            AND (@fromDate IS NULL OR a.createdAt BETWEEN @fromDate AND IFNULL(@toDate, a.createdAt))
            AND (@minPrice IS NULL OR a.price BETWEEN @minPrice AND IFNULL(@maxPrice, a.price))
            AND (@cityIdsString IS NULL OR FIND_IN_SET(c.cityID, @cityIdsString) > 0)
            AND (@provinceIdsString IS NULL OR FIND_IN_SET(c.provinceID, @provinceIdsString) > 0)
            AND (@categoryId IS NULL OR a.ACID = @categoryId)
            AND (@statusId IS NULL OR a.statusID = @statusId)
        ORDER BY a.createdAt DESC
        LIMIT @pageSize OFFSET @offset;
    ";

            int offset = (filter.Page - 1) * filter.PageSize;

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@search", filter.Search ?? "");
                command.Parameters.AddWithValue("@fromDate", filter.FromDate);
                command.Parameters.AddWithValue("@toDate", filter.ToDate);
                command.Parameters.AddWithValue("@minPrice", filter.MinPrice);
                command.Parameters.AddWithValue("@maxPrice", filter.MaxPrice);
                command.Parameters.AddWithValue("@cityIdsString", string.Join(",", filter.CityIds));
                command.Parameters.AddWithValue("@provinceIdsString", string.Join(",", filter.ProvinceIds));
                command.Parameters.AddWithValue("@categoryId", filter.CategoryId);
                command.Parameters.AddWithValue("@statusId", filter.StatusId);
                command.Parameters.AddWithValue("@pageSize", filter.PageSize);
                command.Parameters.AddWithValue("@offset", offset);

                Console.WriteLine("Executed SQL query: " + command.ToString());

                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string pictureUrl = reader.IsDBNull(reader.GetOrdinal("picture")) ? null : reader.GetString("picture");
                            var adPreview = new MySqlAdPreview
                            {
                                AdId = reader.GetInt32("adID"),
                                Title = reader.GetString("title"),
                                CreationDate = reader.GetDateTime("createdAt"),
                                Price = reader.GetDecimal("price"),
                                AdImage = pictureUrl
                            };
                            adPreviews.Add(adPreview);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Error retrieving ad previews.", ex);
                }
            }

            return adPreviews;
        }

        public Ad GetAdById(string adId)
        {
                Ad ad = new Ad();

                string query = @"
                    SELECT 
                        a.adID AS AdId,
                        a.title AS Title,
                        a.description AS Description,
                        a.price AS Price,
                        a.createdAt AS CreatedAt,
                        c.Name AS CityTitle,
                        c.cityID AS CityId,
                        s.Title AS StatusTitle,
                        s.statusID AS StatusId,
                        ac.title AS AdCategoryTitle,
                        ac.ACID AS AdCategoryId
                    FROM ad a
                    JOIN city c ON a.cityID = c.cityID
                    JOIN `status` s ON a.statusID = s.statusID
                    JOIN ad_category ac ON a.ACID = ac.ACID
                    WHERE a.adID = @adId;

                    SELECT 
                        am.name AS Name,
                        am.value AS Value
                    FROM ad_meta am
                    WHERE am.adID = @adId;

                    SELECT 
                        ap.pictureURL AS PictureId
                    FROM ad_pictures ap
                    WHERE ap.adID = @adId;
                ";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@adId", adId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ad.AdId = reader.GetInt32("AdId").ToString();
                        ad.Title = reader.GetString("Title");
                        ad.Description = reader.GetString("Description");
                        ad.Price = reader.GetDecimal("Price");
                        ad.CreatedAt = reader.GetDateTime("CreatedAt");

                        ad.City = new TitleId
                        {
                            Title = reader.GetString("CityTitle"),
                            Id = reader.GetInt32("CityId")
                        };

                        ad.Status = new TitleId
                        {
                            Title = reader.GetString("StatusTitle"),
                            Id = reader.GetInt32("StatusId")
                        };

                        ad.AdCategory = new TitleId
                        {
                            Title = reader.GetString("AdCategoryTitle"),
                            Id = reader.GetInt32("AdCategoryId")
                        };
                    }


                    reader.NextResult();
                    while (reader.Read())
                    {
                        ad.MetaData.Add(reader.GetString("Name"), reader.GetString("Value"));
                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        ad.Images.Add("https://localhost:7261/api/Image/View/" + reader.GetString("PictureId"));
                    }
                }

            }

            return ad;
        }

        public void IncrementAdViewCount(string adID)
        {
            string query = @"
                UPDATE ad_views
                SET viewCount = viewCount + 1
                WHERE adID = @adID;
            ";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@adID", adID);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error incrementing ad view count.", ex);
                }
            }
        }

        public void RecordCustomerAdView(string customerID, string adID)
        {
            string query = @"
                INSERT INTO customer_view_ad (customerID, adID)
                VALUES (@customerID, @adID)
                ON DUPLICATE KEY UPDATE customerID = @customerID, adID = @adID;
            ";

            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@customerID", customerID);
                command.Parameters.AddWithValue("@adID", adID);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions here
                    throw new Exception("Error recording customer ad view.", ex);
                }
            }
        }
    }


}
