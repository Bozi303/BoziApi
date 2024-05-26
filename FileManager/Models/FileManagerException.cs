namespace FileManager.Models
{
    public class FileManagerException : Exception
    {
        public int Code { get; }
        public FileManagerException(int code, string message) : base(message)
        {
            Code = code;
        }
    }
}
