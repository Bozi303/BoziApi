using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.MySql.MySqlModels
{
    public class MySqlAd
    {
        public int AdId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CustomerId { get; set; } 
        public int StoreId { get; set; }
        public int CityId { get; set; }
        public int StatusId { get; set; }
        public int ACID { get; set; }
    }
}
