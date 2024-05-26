namespace FileManager.Models
{
    public interface IFileManager
    {
        public string SaveFile(byte[] data, string name, string dataType);
        public FileModel GetFile(string fileId);
        public void DeleteFile(string fileId);
    }
}
