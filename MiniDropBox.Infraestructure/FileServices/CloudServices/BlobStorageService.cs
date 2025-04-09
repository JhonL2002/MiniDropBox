using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces.FileServices;

namespace MiniDropBox.Infraestructure.FileServices.CloudServices
{
    public class BlobStorageService : IFileStorageService<IFormFile>
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobStorageService(IConfiguration configuration)
        {
            _blobServiceClient = new BlobServiceClient(
                new Uri(configuration["AZURE_CONNECTION_STRINGS:BLOB_STORAGE_URL"]!),
                new DefaultAzureCredential()
            );

            _containerName = configuration["AZURE_CONNECTION_STRINGS:BLOB_CONTAINER"]!;
        }

        private BlobClient CreateBlobClient(string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            return containerClient.GetBlobClient(fileName);
        }

        public async Task<string> UploadStreamAsync(UploadFileDTO<IFormFile> uploadFileDTO)
        {
            var blobClient = CreateBlobClient(uploadFileDTO.File.FileName);

            await using var stream = uploadFileDTO.File.OpenReadStream();

            await blobClient.UploadAsync(stream, overwrite: true);

            var filePath = Path.Combine(uploadFileDTO.FolderPath, blobClient.Name);
            return filePath;
        }

        public async Task<bool> DeleteStreamAsync(string fileName)
        {
            var blobClient = CreateBlobClient(fileName);
            var result = await blobClient.DeleteIfExistsAsync();
            if (!result)
            {
                return false;
            }
            return true;
        }

        public async Task<Stream?> DownloadStreamAsync(string fileName)
        {
            var blobClient = CreateBlobClient(fileName);
            return await blobClient.ExistsAsync() ? await blobClient.OpenReadAsync() : null;
        }
    }
}
