using Infrastructure.DataAccess.MySql;
using Infrastructure.DataAccess.Redis;
using SharedModel.BoziService;
using SharedModel.Models;
using SharedModel.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.BoziService
{
    public class StoreService : IStoreService
    {

        private readonly MySqlDataContext _mySqlDb;
        private readonly RedisDataContext _redisDb;

        public StoreService(MySqlDataContext mySqlDb, RedisDataContext redisDb)
        {
            _mySqlDb = mySqlDb;
            _redisDb = redisDb;
        }

        public bool CheckCustomerIsStoreOwner(string customerId, string storeId)
        {
            try
            {
                var state = _mySqlDb.StoreRepository.IsCustomerStoreOwner(storeId, customerId);
                return state;
            } catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }

        public List<TitleId> GetStoreCategories()
        {
            try
            {
                var categoriesRedis = _redisDb.GetData<List<TitleId>>("store_categories");
                if (categoriesRedis != null)
                {
                    return categoriesRedis;
                }

                var sqlCategories = _mySqlDb.StoreRepository.GetStoreCategories();

                var categories = sqlCategories.Select(c =>
                {
                    return new TitleId
                    {
                        Id = c.Id,
                        Title = c.Title
                    };
                }).ToList();

                _redisDb.SetData("store_categories", categories, 84600);

                return categories;
            }
            catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }

        public string GetStoreRegistrationNumber()
        {
            var random = new Random();

            return random.NextInt64(100000000, 10000000000).ToString();

        }

        public void RegistrationStore(CreateStore request)
        {
            try
            {
                _mySqlDb.StoreRepository.InsertStore(request);
            }
            catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }
    }
}
