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
        private readonly IStoreService _storeService;

        public BoziService(RedisDataContext redisDb, MySqlDataContext mySqlDb)
        {
            _customerService = new CustomerService(redisDb, mySqlDb);
            _cityService = new CityService(mySqlDb, redisDb);
            _adService = new AdService(mySqlDb, redisDb);
            _reportService = new ReportService(mySqlDb, redisDb);
            _storeService = new StoreService(mySqlDb, redisDb);
        }

        public ICustomerService CustomerService => _customerService;
        public ICityService CityService => _cityService;
        public IAdService AdService => _adService;
        public IReportService ReportService => _reportService;
        public IStoreService StoreService => _storeService;
    }
}
