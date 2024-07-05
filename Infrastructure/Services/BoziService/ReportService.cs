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
    public class ReportService : IReportService
    {

        private readonly MySqlDataContext _mySqlDb;
        private readonly RedisDataContext _redisDb;

        public ReportService(MySqlDataContext mySqlDb, RedisDataContext redisDb)
        {
            _mySqlDb = mySqlDb;
            _redisDb = redisDb;
        }

        
        public List<TitleId> GetReportCategories()
        {
            try
            {
                var categoriesRedis = _redisDb.GetData<List<TitleId>>("report_categories");
                if (categoriesRedis != null)
                {
                    return categoriesRedis;
                }

                var categories = _mySqlDb.ReportRepository.GetAllAdReportCategories();

                _redisDb.SetData("reoirt_categories", categories, 84600);
                
                return categories;
            }
            catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }


        public List<TitleId> GetReportStatus()
        {
            try
            {
                var statusesRedis = _redisDb.GetData<List<TitleId>>("report_statuses");
                if (statusesRedis != null)
                {
                    return statusesRedis;
                }
        
                var statuses = _mySqlDb.ReportRepository.GetAllAdReportStatuses();

                _redisDb.SetData("report_statuses", statuses, 84600);

                return statuses;
            } catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }

        public void ReportAd(CreateCustomerReport req)
        {
            try
            {
                _mySqlDb.ReportRepository.CreateCustomerAdReport(req);
            } catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }
        
    }
}
