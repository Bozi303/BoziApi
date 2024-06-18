using SharedModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.BoziService
{
    public interface IAdService 
    {
        public List<Ad> GetAdByCustomerId(string customerId);
    }
}
