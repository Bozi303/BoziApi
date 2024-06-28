namespace ClietnApi.Controllers.PresentationModel
{
    public class RegisterStoreRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? CategoryId { get; set; }
        public string? CityId { get; set; }
    }
}
