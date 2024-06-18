using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.MySql.MySqlModels
{
    public class MySqlProvince
    {
        public int ProvinceId { get; set; }
        public string? ProvinceName { get; set; }
    }
}
