using MiniDropBox.Application.DTOs;

namespace MiniDropBox.Application.Interfaces.FileServices
{
    public interface IFileStorageService
    {
        Task MoveBlobAsync(string oldPath, string newPath);
        Task<string> UploadStreamAsync(UploadFileDTO<IFileUpload> uploadFileDTO);
        Task<bool> DeleteStreamAsync(string fullPath);
        Task<Stream?> DownloadStreamAsync(string fullPath);
    }
}
