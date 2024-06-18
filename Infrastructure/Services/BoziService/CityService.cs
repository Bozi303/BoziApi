using Infrastructure.DataAccess.MySql;
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
        private MySqlDataContext _mySqlDb;
        
        public CityService(MySqlDataContext mySqlDb)
        {
            _mySqlDb = mySqlDb;
        }

        public List<City> GetCitiesByProvinceId(string provinceId)
        {
            try
            {
                var city = _mySqlDb.CityRepository.GetCitiesByProvinceId(provinceId);
                return city.Select(c =>
                {
                    return new City
                    {
                        CityId = c.CityId.ToString(),
                        CityName = c.CityName
                    };
                }).ToList();

            } catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }

        public List<Province> GetProvinces()
        {
            try
            {
                var province = _mySqlDb.CityRepository.GetAllProvinces();
                return province.Select(p =>
                {
                    return new Province
                    {
                        ProvinceId = p.ProvinceId.ToString(),
                        ProvinceName = p.ProvinceName
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }
    }
}
