namespace ClietnApi.Controllers.PresentationModel
{
    public class RegisterCustomerRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? CityId { get; set; }
    }
}
