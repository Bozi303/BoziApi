namespace FileManager.Controllers
{
    public class AddFileRequest
    {
        public byte[] Data { get; set; }
        public string name { get; set; }
        public string extension { get; set; }
    }
}
