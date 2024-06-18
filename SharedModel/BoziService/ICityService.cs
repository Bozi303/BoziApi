using SharedModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.BoziService
{
    public interface ICityService
    {
        List<Province> GetProvinces();
        List<City> GetCitiesByProvinceId(string provinceId);
    }
}
