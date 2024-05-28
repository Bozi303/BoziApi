using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.Authentication
{
    public class CaptchaValues
    {
        public string? CaptchaKey { get; set; }
        public string? CaptchaValue { get; set; }
        public byte[] ImageText { get; set; } = Array.Empty<byte>();
    }
}
