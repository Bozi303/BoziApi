using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.ElasticSearch.ElasticSearchModel
{
    public class ElasticAdPreviewModel
    {
        public string? AdId { get; set; }
        public string? Title { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal Price { get; set; }
        public string? AdImage { get; set; }

        public string? CityId { get; set; }
        public string? CategoryId { get; set; }
    }
}
