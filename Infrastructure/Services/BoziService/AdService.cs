using Infrastructure.DataAccess.MySql;
using SharedModel.BoziService;
using SharedModel.Models;
using SharedModel.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Infrastructure.Services.BoziService
{
    public class AdService : IAdService
    {

        private readonly MySqlDataContext _mySqlDb;

        public AdService(MySqlDataContext mySqlDb)
        {
            _mySqlDb = mySqlDb;
        }

        public List<Ad> GetAdByCustomerId(string customerId)
        {
            try
            {
                var mySqlAd = _mySqlDb.AdRepository.GetAdRepositoriesByCustomerId(customerId);
                //return ad;
                throw new NotImplementedException();
            }
            catch (BoziException ex)
            {
                throw ex;
            }
        }

        public List<string> GetMetaKeysByCategoryId(string categoryId)
        {
            try
            {
                var keys = _mySqlDb.AdRepository.GetMetaKeysByCategoryId(categoryId);
                return keys;
            }
            catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }

        public List<AdCategory> GetAdCategories(string parentId)
        {
            try
            {
                var categories = _mySqlDb.AdRepository.GetCategoriesByParentId(parentId);

                return categories.Select(s =>
                {
                    return new AdCategory
                    {
                        ACID = s.ACID.ToString(),
                        Title = s.Title
                    };
                }).ToList();

            }
            catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }

        public List<AdPreview> GetAdsPreview(GetAdPreview getAdPreview)
        {
            try
            {
                var preview = _mySqlDb.AdRepository.GetAdPreviews(getAdPreview);
                return preview.Select(a =>
                {
                    return new AdPreview
                    {
                        AdId = a.AdId.ToString(),
                        CreationDate = a.CreationDate,
                        AdImage = string.IsNullOrEmpty(a.AdImage) ? "" : "https://localhost:7261/api/Image/View/" + a.AdImage,
                        Price = a.Price,
                        Title = a.Title
                    };
                }).ToList();
            } catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }

        public Ad GetAdById(string adId, string viewerId)
        {
            try
            {
                var ad = _mySqlDb.AdRepository.GetAdById(adId);

                _mySqlDb.AdRepository.IncrementAdViewCount(adId);

                _mySqlDb.AdRepository.RecordCustomerAdView(viewerId, adId);

                return ad;

            } catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }
    }
}
