using Microsoft.AspNetCore.Mvc.Formatters;

namespace ClietnApi.Controllers.PresentationModel
{

    public class AdPreviewRequest
    {
        public string? Search { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<string> CityIds { get; set; } = new();
        public List<string> ProvinceIds { get; set; } = new();
        public string? CategoryId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
