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
    public class AdService : IAdService
    {

        private readonly MySqlDataContext _mySqlDb;

        public AdService(MySqlDataContext mySqlDb)
        {
            _mySqlDb = mySqlDb;
        }

        public List<Ad> GetAdByCustomerId(string customerId)
        {
            try
            {
                var mySqlAd = _mySqlDb.AdRepository.GetAdRepositoriesByCustomerId(customerId);
                //return ad;
                throw new NotImplementedException();
            } catch (BoziException ex)
            {
                throw ex;
            }
        }
    }
}
