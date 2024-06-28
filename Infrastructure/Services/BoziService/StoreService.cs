using Infrastructure.DataAccess.MySql;
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

        public StoreService(MySqlDataContext mySqlDb)
        {
            _mySqlDb = mySqlDb;
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
                var categories = _mySqlDb.StoreRepository.GetStoreCategories();

                return categories.Select(c =>
                {
                    return new TitleId
                    {
                        Id = c.Id,
                        Title = c.Title
                    };
                }).ToList();
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
