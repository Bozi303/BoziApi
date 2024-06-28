using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.Models
{
    public enum StoreStatusEnum
    {
        None = 0,
        Waiting = 1,
        Accepted = 2,
        Deleted = 3,
        Locked = 4
    }
}
