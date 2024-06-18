using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.MySql.MySqlModels
{
    public class MySqlCity
    {
        public int CityId { get; set; }
        public string? CityName { get; set; }
    }
}
