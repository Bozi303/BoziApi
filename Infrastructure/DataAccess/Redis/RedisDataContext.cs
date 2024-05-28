using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.Redis
{
    public class RedisDataContext
    {
        private readonly IDatabase _redisCache;

        public RedisDataContext(string connectionString)
        {
            var configuration = ConfigurationOptions.Parse(connectionString);
            var redisConnection = ConnectionMultiplexer.Connect(configuration);

            _redisCache = redisConnection.GetDatabase();
        }

        public void SetData<T>(string key, T value, int ttl = 600)
        {
            try
            {
                _redisCache.StringSet(key, JsonConvert.SerializeObject(value), TimeSpan.FromSeconds(ttl));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public T? GetData<T>(string key)
        {
            try
            {
                var value = _redisCache.StringGet(key);

                if (value.HasValue)
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }

                return default;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
