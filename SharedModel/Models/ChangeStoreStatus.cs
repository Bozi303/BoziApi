using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.Models
{
    public class ChangeStoreStatus
    {
        public string? StoreId { get; set; }
        public string? StatusId { get; set; }
        public string? Note { get; set; }
    }
}
