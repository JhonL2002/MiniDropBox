using MiniDropBox.Application.DTOs;

namespace MiniDropBox.Application.Interfaces.FileServices
{
    public interface IFileStorageService<T>
    {
        Task<string> UploadStreamAsync(UploadFileDTO<T> uploadFileDTO);
        Task<bool> DeleteStreamAsync(string filePath);
        Task<Stream?> DownloadStreamAsync(string filePath);
    }
}
