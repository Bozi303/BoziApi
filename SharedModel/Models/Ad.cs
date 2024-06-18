using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.Models
{
    public class Ad
    {
        public string? adId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public TitleId City { get; set; } = new();
        public TitleId Status { get; set; } = new();
        public TitleId AdCategory { get; set; } = new();
        public Dictionary<string, string> MetaData { get; set; } = new();
        public List<AdImage> Images { get; set; } = new();
    }
}
