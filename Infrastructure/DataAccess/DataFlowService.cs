using Infrastructure.DataAccess.Redis;
using SharedModel.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess
{
    public class DataFlowService
    {
        private readonly RedisDataContext _redisDb;

        public DataFlowService(RedisDataContext redisDataContext) 
        {
            _redisDb = redisDataContext;
        }

        public void GenerateOTP(string mobileNumber, string otp)
        {
            try
            {
                _redisDb.SetData(mobileNumber, otp, 120);
            } catch (Exception ex)
            {
                throw new BoziException(500, ex.Message);
            }
        }

        public string? VerifyOTP(string mobileNumber)
        {
            try
            {
                var storedOTP = _redisDb.GetData<string>(mobileNumber);

                return storedOTP;

            } catch (Exception ex)
            {
                throw new BoziException(500, ex.Message);
            }
        }
    }
}
