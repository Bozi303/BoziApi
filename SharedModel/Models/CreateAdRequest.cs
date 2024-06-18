using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.Models
{
    public class CreateAdRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? CustomerId { get; set; }
        public string? StoreId { get; set; }
        public int StatusId { get; set; }
        public string? AdCategoryId { get; set; }
        public string? CityId { get; set; }

    }
}
