﻿using Infrastructure.DataAccess.ElasticSearch.ElasticSearchModel;
using Infrastructure.DataAccess.MySql;
using Infrastructure.DataAccess.Redis;
using Infrastructure.Model;
using Nest;
using SharedModel.BoziService;
using SharedModel.Models;
using SharedModel.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.BoziService
{
    public class CustomerService : ICustomerService
    {

        private readonly IElasticClient _elastic;    
        private readonly RedisDataContext _redisDb;
        private readonly MySqlDataContext _mySql;

        public CustomerService(RedisDataContext redis, MySqlDataContext mySql, IElasticClient elasticClient)
        {
            _redisDb = redis;
            _mySql = mySql;
            _elastic = elasticClient;
        }

        public CustomerProfile GetCustomerProfile(string customerId)
        {
            var customer = _mySql.CustomerRepository.GetCustomerById(customerId) ?? throw new BoziException(400, "مشتری پیدا نشد");

            return new CustomerProfile
            {
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                MobileNumber = customer.MobileNumber
            };
        }

        public void CreateCustomer(CreateCustomerRequest req)
        {
            try
            {
                _mySql.CustomerRepository.CreateCustomer(req);
            } catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }

        public void AdRegistration(CreateAdRequest req)
        {
            try
            {
                var adID = _mySql.AdRepository.CreateAd(req);

                foreach (var meta in req.MetaDatas)
                {
                    _mySql.AdRepository.CreateAdMeta(adID, meta.Key, meta.Value);
                }

                _mySql.AdRepository.CreateAdView(adID, 0);

                foreach (var pictureId in req.PicutresIds)
                {
                    _mySql.AdRepository.CreateAdPicture(adID, pictureId);
                }

                // insert in elasticSearch

                _elastic.IndexDocument(new ElasticAdPreviewModel
                {
                    AdId = adID.ToString(),
                    AdImage = req.PicutresIds.FirstOrDefault(),
                    CreationDate = DateTime.Now,
                    Price = req.Price,
                    Title = req.Title,
                    CategoryId = req.AdCategoryId,
                    CityId = req.CityId
                });
            }
            catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }


        public void UpdateCustomerProfile(EditCustomerProfile customerProfile)
        {
            try
            {
                //TODO
                throw new NotImplementedException();
            } catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }

        /* public List<Ad> GetAdByCustomerID(string customerId)
         {
             try
             {
                 var sqlAds = _mySql.AdRepository.GetAdRepositoriesByCustomerId(customerId);



             } catch (BoziException ex)
             {

             }
         }*/
    }
}
