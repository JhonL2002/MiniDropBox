using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces.FileServices;

namespace MiniDropBox.Infraestructure.FileServices.CloudServices
{
    public class BlobStorageService : IFileStorageService
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

        private BlobClient CreateBlobClient(string fullPath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            return containerClient.GetBlobClient(fullPath);
        }

        public async Task<string> UploadStreamAsync(UploadFileDTO<IFileUpload> uploadFileDTO)
        {
            await using var stream = uploadFileDTO.File.OpenReadStream();

            var fullBlobPath = Path.Combine(uploadFileDTO.FolderPath, uploadFileDTO.File.FileName).Replace("\\","/");

            var blobClient = CreateBlobClient(fullBlobPath);

            await blobClient.UploadAsync(stream, overwrite: true);

            return fullBlobPath;
        }

        public async Task<bool> DeleteStreamAsync(string filePath)
        {
            var blobClient = CreateBlobClient(filePath);
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

        public async Task MoveBlobAsync(string oldPath, string newPath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            var sourceBlob = containerClient.GetBlobClient(oldPath);
            var destinationBlob = containerClient.GetBlobClient(newPath);

            // Verify if original blob exists
            if (!await sourceBlob.ExistsAsync())
                throw new FileNotFoundException($"Blob not found at path: {oldPath}");

            // Copy to new blob
            await destinationBlob.StartCopyFromUriAsync(sourceBlob.Uri);

            // Delete the original blob
            await sourceBlob.DeleteIfExistsAsync();
        }
    }
}
