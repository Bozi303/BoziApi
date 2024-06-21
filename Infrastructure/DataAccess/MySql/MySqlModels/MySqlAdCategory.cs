using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.MySql.MySqlModels
{
    public class MySqlAdCategory
    {
        public int ACID { get; set; }
        public string? ParentId { get; set; }
        public string? Title { get; set; }
        public bool IsLeaf { get; set; }
    }
}
