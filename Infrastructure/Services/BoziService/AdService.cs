using Infrastructure.DataAccess.ElasticSearch.ElasticSearchModel;
using Infrastructure.DataAccess.MySql;
using Infrastructure.DataAccess.Redis;
using Nest;
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
        private readonly RedisDataContext _redisDb;
        private readonly IElasticClient _elastic;
        public AdService(MySqlDataContext mySqlDb, RedisDataContext redisDb, IElasticClient elasticClient)
        {
            _mySqlDb = mySqlDb;
            _redisDb = redisDb;
            _elastic = elasticClient;
        }

        public List<Ad> GetAdByCustomerId(string customerId)
        {
            try
            {
                var mySqlAd = _mySqlDb.AdRepository.GetAdRepositoriesByCustomerId(customerId);
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
                var categoryRedis = _redisDb.GetData<List<AdCategory>>($"category_{parentId}");

                if (categoryRedis != null)
                {
                    return categoryRedis;
                }

                var sqlCategories = _mySqlDb.AdRepository.GetCategoriesByParentId(parentId);


                var categories = sqlCategories.Select(s =>
                {
                    return new AdCategory
                    {
                        ACID = s.ACID.ToString(),
                        Title = s.Title
                    };
                }).ToList();

                _redisDb.SetData($"category_{parentId}", categories, 86400);

                return categories;

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
                // first search in elastisearch
                var search = getAdPreview.Search;
                var cities = getAdPreview.CityIds;
                var minPrice = getAdPreview.MinPrice;
                var maxPrice = getAdPreview.MaxPrice;
                var categoryId = getAdPreview.CategoryId;
                
                var preview = ElasticsearchFilter(search, cities, minPrice, maxPrice, categoryId);
                if (preview.Any())
                {
                    return preview;
                }

                var sqlPreview = _mySqlDb.AdRepository.GetAdPreviews(getAdPreview);
                return sqlPreview.Select(a =>
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

        private List<AdPreview> ElasticsearchFilter(string search, List<string> cities, decimal minPrice, decimal maxPrice, string categoryId)
        {
            ISearchResponse<ElasticAdPreviewModel> searchResponse;

            searchResponse = _elastic.Search<ElasticAdPreviewModel>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Title)
                        .Query(search)
                    )
                )
            );

            if (cities.Count > 0)
            {
                searchResponse = _elastic.Search<ElasticAdPreviewModel>(s => s
                    .Query(q => q
                        .Bool(b => b
                            .Must(
                                m => m.Match(mm => mm.Field(f => f.Title).Query(search))
                                )
                            .Should(
                            s => s.Terms(t => t.Field(f => f.CityId).Terms(cities))
                            )
                            .Filter(
                            f => f.Range(r => r.Field(ff => ff.Price).GreaterThanOrEquals((double?)minPrice)),
                            f => f.Range(r => r.Field(ff => ff.Price).LessThanOrEquals((double?)maxPrice))
                            )
                        )
                    )
                );
            } else
            {
                searchResponse = _elastic.Search<ElasticAdPreviewModel>(s => s
                    .Query(q => q
                        .Bool(b => b
                            .Must(
                                m => m.Match(mm => mm.Field(f => f.Title).Query(search))
                                )
                                .Filter(
                                    f => f.Range(r => r.Field(ff => ff.Price).GreaterThanOrEquals((double?)minPrice)),
                                    f => f.Range(r => r.Field(ff => ff.Price).LessThanOrEquals((double?)maxPrice))
                                )
                            )
                         )
                );
            }


            if (searchResponse.Documents.Any())
            {
                var res = searchResponse.Documents.ToList();
                return res.Select(s =>
                {
                    return new AdPreview
                    {
                        AdId = s.AdId,
                        AdImage = s.AdImage,
                        CreationDate = s.CreationDate,
                        Price = s.Price,
                        Title = s.Title
                    };
                }).ToList();
            } else
            {
                return new();
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

        public void ChangeAdStatus(ChangeAdStatus changeAdStatus, string adminId)
        {
            try
            {
                _mySqlDb.AdRepository.UpdateAdStatus(adminId, changeAdStatus.StatusId);
                _mySqlDb.AdRepository.InsertSupportAdStatus(changeAdStatus.Note, adminId, changeAdStatus.AdId, changeAdStatus.StatusId);
            } catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }

        public List<TitleId> GetAdStatuses()
        {
            try
            {
                var statuses = _mySqlDb.AdRepository.GetAllStatus();
                return statuses;
            } catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }
    }
}
