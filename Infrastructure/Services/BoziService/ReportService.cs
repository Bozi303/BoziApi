using Infrastructure.DataAccess.MySql;
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

        public ReportService(MySqlDataContext mySqlDb)
        {
            _mySqlDb = mySqlDb;
        }

        
        public List<TitleId> GetReportCategories()
        {
            try
            {
                var categories = _mySqlDb.ReportRepository.GetAllAdReportCategories();
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
                var statuses = _mySqlDb.ReportRepository.GetAllAdReportStatuses();
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
