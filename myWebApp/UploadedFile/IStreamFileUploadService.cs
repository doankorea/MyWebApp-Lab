using Microsoft.AspNetCore.WebUtilities;

namespace myWebApp.UploadedFile
{
    public interface IStreamFileUploadService
    {
        Task<bool> UploadFile(MultipartReader reader, MultipartSection section);
    }
}
