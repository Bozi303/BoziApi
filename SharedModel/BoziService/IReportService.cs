using SharedModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.BoziService
{
    public interface IReportService
    {
        List<TitleId> GetReportCategories();
        List<TitleId> GetReportStatus();
        void ReportAd(CreateCustomerReport req);
    }
}
