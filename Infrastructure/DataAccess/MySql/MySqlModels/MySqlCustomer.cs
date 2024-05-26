using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.MySql.MySqlModels
{
    public class MySqlCustomer
    {
        public int CustomerID { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime LastAccess { get; set; }
        public DateTime RegisterTime { get; set; }
        public int CSID { get; set; }
        public int CityID { get; set; }
    }
}
