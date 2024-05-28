using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModel.Models;

namespace SharedModel.BoziService
{
    public interface ICustomerService
    {
        public CustomerProfile GetCustomerProfile(string customerId);
    }
}
