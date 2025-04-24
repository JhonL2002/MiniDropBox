using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Application.Interfaces.FileServices;
using MiniDropBox.Application.Interfaces.UnitOfWork;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;
using File = MiniDropBox.Core.Models.File;

namespace MiniDropBox.Application.Implementations
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly IFileStorageService _fileStorageService;

        public FileService(IFileRepository fileRepository, IFolderRepository folderRepository, IFileStorageService fileStorageService)
        {
            _fileRepository = fileRepository;
            _folderRepository = folderRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<string>> DeleteFileAsync(int fileId, int userId)
        {
            var file = await _fileRepository.GetByIdAsync(fileId);
            if (file == null || file.UserId != userId)
                return Result<string>.Failure("File not found or you don't have permission");

            await _fileRepository.DeleteAsync(fileId);

            var isDeleted = await _fileStorageService.DeleteStreamAsync(file.Path);
            if (!isDeleted)
                return Result<string>.Failure("Blob not found");

            return Result<string>.Success($"File {file.Name} deleted successfully");
        }

        public async Task<Result<(Stream stream, string fileName)>> DownloadFileAsync(int fileId, int userId)
        {
            var file = await _fileRepository.GetByIdAsync(fileId);
            if (file == null || file.UserId != userId)
                return Result<(Stream stream, string fileName)>.Failure("File not found or you don't have permission");

            var stream = await _fileStorageService.DownloadStreamAsync(file.Path);
            if (stream == null)
                return Result<(Stream, string)>.Failure("Blob not found");

            return Result<(Stream, string)>.Success((stream, file.Name));
        }

        public async Task<Result<string>> MoveFileAsync(MoveFileDTO moveFileDTO, int userId)
        {
            var file = await _fileRepository.GetByIdAsync(moveFileDTO.Id);
            if (file == null || file.UserId != userId)
                return Result<string>.Failure("File not found or you don't have permission");

            var targetFolder = await _folderRepository.GetByIdAsync(moveFileDTO.FolderId);
            if (targetFolder == null || targetFolder.UserId != userId)
                return Result<string>.Failure("Target folder not found");

            // Build the new file's path 
            var newPath = Path.Combine(targetFolder.Path, file.Name).Replace("\\","/");

            // Move to blob
            await _fileStorageService.MoveBlobAsync(file.Path, newPath);

            // Update the file's path
            file.Path = newPath;
            await _fileRepository.UpdateAsync(file);

            return Result<string>.Success($"File moved to: {file.Path}");
        }

        public async Task<Result<FileDTO>> UploadFileAsync(UploadFileDTO<IFileUpload> uploadFileDTO, int userId)
        {
            var folder = await _folderRepository.GetByIdAsync(uploadFileDTO.FolderId);
            if (folder == null || folder.UserId != userId)
                return Result<FileDTO>.Failure("Invalid Folder");

            // Build the path for blob
            var folderPath = Path.Combine(folder.Path).Replace("\\","/");

            var dtoWithPath = new UploadFileDTO<IFileUpload>(
                uploadFileDTO.File,
                uploadFileDTO.FolderId,
                folderPath
            );

            var path = await _fileStorageService.UploadStreamAsync(dtoWithPath);

            var file = new File
            {
                Name = uploadFileDTO.File.FileName,
                Size = uploadFileDTO.File.Lenght,
                Extension = Path.GetExtension(uploadFileDTO.File.FileName),
                Path = path,
                FolderId = folder.Id,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            var createdFile = await _fileRepository.AddAsync(file);

            return Result<FileDTO>.Success(new FileDTO
            (
                createdFile.Name,
                createdFile.Size,
                createdFile.Extension,
                createdFile.Path,
                createdFile.FolderId,
                createdFile.UserId
            ));
        }
    }
}
