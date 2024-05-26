using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.System
{
    public class BoziException : Exception
    {
        public int ErrorCode { get; }

        public BoziException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
