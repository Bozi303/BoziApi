using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Model;
using SharedModel.Models;

namespace SharedModel.BoziService
{
    public interface ICustomerService
    {
        CustomerProfile GetCustomerProfile(string customerId);
        void AdRegistration(CreateAdRequest req);
        void CreateCustomer(CreateCustomerRequest req);
    }
}
