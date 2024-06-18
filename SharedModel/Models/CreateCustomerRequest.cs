using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Model
{
    public class CreateCustomerRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? CityId { get; set; }
        public DateTime RegisterationDate { get; set; }
        public DateTime LastAccess { get; set; }
        public int CustomerStatus { get; set; }
    }
}
