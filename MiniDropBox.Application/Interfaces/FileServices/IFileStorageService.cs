using MiniDropBox.Application.DTOs;

namespace MiniDropBox.Application.Interfaces.FileServices
{
    public interface IFileStorageService<T>
    {
        Task<string> UploadFileAsync(UploadFileDTO<T> uploadFileDTO);
        Task<bool> DeleteFileAsync(string filePath);
        Task<Stream?> GetFileAsync(string filePath);
    }
}
