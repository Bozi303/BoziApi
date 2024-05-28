using System.ComponentModel.DataAnnotations;

namespace ClietnApi.Controllers.PresentationModel
{
    public class SendOtpRequest
    {
        [Required]
        public string? MobileNumber { get; set; }
        [Required]
        public string? CaptchaKey { get; set; }
        [Required]
        public string? CaptchaSolve { get; set; }
    }
}
