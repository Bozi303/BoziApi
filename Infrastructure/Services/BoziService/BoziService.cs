using Infrastructure.DataAccess.MySql;
using Infrastructure.DataAccess.Redis;
using SharedModel.BoziService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.BoziService
{
    public class BoziService : IBoziService
    {
        private readonly ICustomerService _customerService;
        private readonly ICityService _cityService;
        private readonly IAdService _adService;
        private readonly IReportService _reportService;

        public BoziService(RedisDataContext redisDb, MySqlDataContext mySqlDb)
        {
            _customerService = new CustomerService(redisDb, mySqlDb);
            _cityService = new CityService(mySqlDb);
            _adService = new AdService(mySqlDb);
            _reportService = new ReportService(mySqlDb);
        }

        public ICustomerService CustomerService => _customerService;
        public ICityService CityService => _cityService;
        public IAdService AdService => _adService;
        public IReportService ReportService => _reportService;
    }
}
