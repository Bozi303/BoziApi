﻿using Infrastructure.DataAccess.MySql;
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

        public BoziService(RedisDataContext redisDb, MySqlDataContext mySqlDb)
        {
            _customerService = new CustomerService(redisDb, mySqlDb);
            _cityService = new CityService(mySqlDb);
        }


        public ICustomerService CustomerService => _customerService;
        public ICityService CityService => _cityService;

    }
}
