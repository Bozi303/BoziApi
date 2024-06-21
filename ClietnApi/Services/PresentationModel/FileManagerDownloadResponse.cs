namespace ClietnApi.Services.PresentationModel
{
    public class FileManagerDownloadResponse
    {
        public string? Name { get; set; }
        public string? FileExtension { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] Data { get; set; } = Array.Empty<byte>();
    }
}
