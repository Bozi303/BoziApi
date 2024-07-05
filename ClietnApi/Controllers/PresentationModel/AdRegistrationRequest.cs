using Microsoft.AspNetCore.Mvc.Formatters;
using SharedModel.Models;

namespace ClietnApi.Controllers.PresentationModel
{
    public class AdRegistrationRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? AdCategoryId { get; set; }
        public string? CityId { get; set; }
        public List<KeyValue> MetaDatas { get; set; } = new();
        public List<string> PictureIds { get; set; } = new();
    }

    public class StoreAdRegistrationRequest : AdRegistrationRequest
    {
        public string? StoreId { get; set; }
    }
}
