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
        public List<string> GetMetaKeysByCategoryId(string categoryId);
        public List<AdCategory> GetAdCategories(string parentId);
        public List<AdPreview> GetAdsPreview(GetAdPreview getAdPreview);
        public Ad GetAdById(string adId);
    }
}
