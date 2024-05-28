using Infrastructure.DataAccess.MySql;
using Infrastructure.DataAccess.Redis;
using SharedModel.BoziService;
using SharedModel.Models;
using SharedModel.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.BoziService
{
    public class CustomerService : ICustomerService
    {

        private readonly RedisDataContext _redisDb;
        private readonly MySqlDataContext _mySql;

        public CustomerService(RedisDataContext redis, MySqlDataContext mySql)
        {
            _redisDb = redis;
            _mySql = mySql;
        }

        public CustomerProfile GetCustomerProfile(string customerId)
        {
            var customer = _mySql.CustomerRepository.GetCustomerById(customerId) ?? throw new BoziException(400, "مشتری پیدا نشد");

            return new CustomerProfile
            {
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                MobileNumber = customer.MobileNumber
            };
        }
    }
}
