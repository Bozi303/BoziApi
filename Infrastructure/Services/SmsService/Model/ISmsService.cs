using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.SmsService.Model
{
    public interface ISmsService
    {
        Task SendSms(string mobileNumber, string message);
    }
}
