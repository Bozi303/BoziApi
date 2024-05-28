using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.SmsService.Model
{
    public class SmsIRSendRequest
    {
        public long LineNumber { get; set; }
        public string? MessageText { get; set; }
        public string[] Mobiles { get; set; } = Array.Empty<string>();
    }
}
