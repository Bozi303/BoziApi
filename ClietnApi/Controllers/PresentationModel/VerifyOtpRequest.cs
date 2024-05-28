namespace ClietnApi.Controllers.PresentationModel
{
    public class VerifyOtpRequest
    {
        public string? MobileNumber { get; set; }
        public string? Otp { get; set; }
    }
}
