using Microsoft.AspNetCore.Http;
using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces.FileServices;
using MiniDropBox.Core.Repositories;

namespace MiniDropBox.Infraestructure.FileServices
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _basePath = "E:/MiniDropBox/Root";

        public Task<bool> DeleteStreamAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<Stream?> DownloadStreamAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                return Task.FromResult<Stream?>(new FileStream(filePath, FileMode.Open, FileAccess.Read));
            }
            return Task.FromResult<Stream?>(null);
        }

        public async Task MoveBlobAsync(string oldPath, string newPath)
        {
            // You need to implement this method to move the file from oldPath to newPath in your local storage.
            await Task.CompletedTask;
        }

        public async Task<string> UploadStreamAsync(UploadFileDTO<IFileUpload> fileDTO)
        {
            var directoryPath = Path.Combine(_basePath, fileDTO.FolderPath);

            Directory.CreateDirectory(directoryPath); // Make folder exists

            var filePath = Path.Combine(directoryPath, fileDTO.File.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileDTO.File.OpenReadStream().CopyToAsync(stream);
            }

            return filePath;
        }
    }
}
