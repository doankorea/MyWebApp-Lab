using myWebApp.UploadedFile;

namespace myWebApp.UploadedFile
{
    public interface IBufferedFileUploadService
    {
        Task<bool> UploadFile(IFormFile file);
    }
}
