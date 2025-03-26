using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Core.Repositories;
using File = MiniDropBox.Core.Models.File;

namespace MiniDropBox.Application.Implementations
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<FileDTO> UploadFileAsync(FileDTO fileDTO)
        {
            var file = new File
            {
                Name = fileDTO.Name,
                Size = fileDTO.Size,
                Extension = fileDTO.Extension,
                Path = fileDTO.Path,
                FolderId = fileDTO.FolderId,
                UserId = fileDTO.UserId,
                CreatedAt = DateTime.UtcNow
            };

            var createdFile = await _fileRepository.AddAsync(file);

            return new FileDTO
            (
                createdFile.Name,
                createdFile.Size,
                createdFile.Extension,
                createdFile.Path,
                createdFile.UserId,
                createdFile.FolderId
            );
        }
    }
}
