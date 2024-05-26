namespace FileManager.Models
{
    public class FileModel
    {
        public string? Name { get; set; }
        public string? FileExtension { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] Data { get; set; } = Array.Empty<byte>();
    }
}
