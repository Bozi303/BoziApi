namespace ClietnApi.Controllers.PresentationModel
{
    public class CaptchaResponse
    {
        public string? CaptchaKey { get; set; }
        public byte[]? CaptchaImage { get; set; }
    }
}
