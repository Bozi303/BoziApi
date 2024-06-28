using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.Models
{
    public class CreateStore
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? RegisterNumber { get; set; }
        public string? CustomerID { get; set; }
        public string? CategoryId { get; set; }
        public string? StatusId { get; set; }
        public string? CityId { get; set; }
    }
}
