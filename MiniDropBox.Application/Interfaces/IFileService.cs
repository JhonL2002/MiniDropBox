using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces.FileServices;

namespace MiniDropBox.Application.Interfaces
{
    public interface IFileService
    {
        public Task<Result<FileDTO>> UploadFileAsync(UploadFileDTO<IFileUpload> uploadFileDTO, int userId);
        public Task<Result<string>> MoveFileAsync(MoveFileDTO moveFileDTO, int userId);
        public Task<Result<string>> DeleteFileAsync(int fileId, int userId);
        public Task<Result<(Stream stream, string fileName)>> DownloadFileAsync(int fileId, int userId);
    }
}
