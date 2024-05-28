using Infrastructure.Services.SmsService.Model;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.SmsService.SmsServiceImplementation
{
    public class SmsIRImplementation : HttpClient, ISmsService
    {

        private readonly long _lineNumber;
        private readonly string _sendUrl;


        public SmsIRImplementation(string sendUrl, long lineNumber, string token)
        {
            _lineNumber = lineNumber;
            _sendUrl = sendUrl;
            DefaultRequestHeaders.Add("X-API-KEY", token);
        }

        public async Task SendSms(string mobileNumber, string message)
        {
            var req = new SmsIRSendRequest
            {
                LineNumber = _lineNumber,
                MessageText = message,
                Mobiles = new string[] { mobileNumber }
            };

            string requestData = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            HttpContent content = new StringContent(requestData, Encoding.UTF8, "application/json");

            await PostAsync(_sendUrl, content);

        }
    }
}
