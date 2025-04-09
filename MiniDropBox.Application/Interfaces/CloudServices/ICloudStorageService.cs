namespace MiniDropBox.Application.Interfaces.CloudServices
{
    public interface ICloudStorageService
    {
        Task UploadStreamAsync(string fileName, Stream fileStream);
        Task DeleteStreamAsync(string fileName);
        Task<Stream?> DownloadStreamAsync(string fileName);
    }
}
