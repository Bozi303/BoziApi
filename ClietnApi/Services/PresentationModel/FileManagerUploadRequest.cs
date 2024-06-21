namespace ClietnApi.Services.PresentationModel
{
    public class FileManagerUploadRequest
    {
        public byte[] Data { get; set; } = Array.Empty<byte>();
        public string? Name { get; set; }
        public string? Extension { get; set; }   
    }
}
