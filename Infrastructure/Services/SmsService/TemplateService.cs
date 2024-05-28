using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.SmsService
{
    public class TemplateService
    {
        public TemplateService()
        {
        }
        public string SMS_loginWithOTP(int TemplateType, string OTPCode, string dateTime)
        {
            string template = "کد ورود: @@OTP@@";
            var res = template
                .Replace("@@OTP@@", OTPCode);
            return res.ToString();
        }
    }
}
