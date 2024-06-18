using Microsoft.AspNetCore.Mvc.Formatters;

namespace ClietnApi.Controllers.PresentationModel
{
    public class AdRegistrationRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? AdCategoryId { get; set; }
        public string? CityId { get; set; }
    }
}
