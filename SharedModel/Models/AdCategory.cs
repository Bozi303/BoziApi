using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.Models
{
    public class AdCategory
    {
        public string ACID { get; set; }
        public string ParentId { get; set; }
        public string Title { get; set; }
        public string IsLeaf { get; set; }

    }
}
