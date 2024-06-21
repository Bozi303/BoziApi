using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.Models
{
    public class AdPreview
    {
        public string? AdId { get; set; }
        public string? Title { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal Price { get; set; }
        public string? AdImage { get; set; }
    }
}
