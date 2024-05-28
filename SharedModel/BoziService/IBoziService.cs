using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.BoziService
{
    public interface IBoziService
    {
        ICustomerService CustomerService { get; }
    }
}
