using Infrastructure.DataAccess.MySql.MySqlModels;
using SharedModel.Models;
using SharedModel.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helper
{
    public class Mapper
    {
        /*public List<Ad> MapSqlAdToAd(List<MySqlAd> mySqlAds)
        {
            try
            {
                return mySqlAds.Select(a =>
                {
                    return new Ad
                    {
                        Title = a.Title,
                        Price = a.Price,
                        Description = a.Description,
                        CreatedAt = a.CreatedAt,
                        AdCategory = 
                    };
                }).ToList();
            } catch (Exception ex)
            {
                throw new BoziException(500, ex.Message); 
            }
        }*/
    }
}
