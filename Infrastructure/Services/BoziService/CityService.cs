using Infrastructure.DataAccess.MySql;
using Infrastructure.DataAccess.Redis;
using SharedModel.BoziService;
using SharedModel.Models;
using SharedModel.System;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.BoziService
{
    public class CityService : ICityService
    {
        private readonly MySqlDataContext _mySqlDb;
        private readonly RedisDataContext _redisDb;

        public CityService(MySqlDataContext mySqlDb, RedisDataContext redisDataContext)
        {
            _mySqlDb = mySqlDb;
            _redisDb = redisDataContext;
        }

        public List<City> GetCitiesByProvinceId(string provinceId)
        {
            try
            {
                var cityRedis = _redisDb.GetData<List<City>>($"city_{provinceId}");

                if (cityRedis != null)
                {
                    return cityRedis;
                }

                var sqlCity = _mySqlDb.CityRepository.GetCitiesByProvinceId(provinceId);

                var city = sqlCity.Select(c =>
                {
                    return new City
                    {
                        CityId = c.CityId.ToString(),
                        CityName = c.CityName
                    };
                }).ToList();

                _redisDb.SetData($"city_{provinceId}", city, 86400);

                return city;
            }
            catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }

        public List<Province> GetProvinces()
        {
            try
            {
                var provinceRedis = _redisDb.GetData<List<Province>>("province");
                
                if (provinceRedis != null)
                {
                    return provinceRedis;
                } 

                var sqlProvince = _mySqlDb.CityRepository.GetAllProvinces();

                var province = sqlProvince.Select(p =>
                {
                    return new Province
                    {
                        ProvinceId = p.ProvinceId.ToString(),
                        ProvinceName = p.ProvinceName
                    };
                }).ToList();
                
                _redisDb.SetData("province", province, 86400);

                return province;
            }
            catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }
    }
}
