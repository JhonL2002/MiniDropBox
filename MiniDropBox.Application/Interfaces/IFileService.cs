using MiniDropBox.Application.DTOs;

namespace MiniDropBox.Application.Interfaces
{
    public interface IFileService
    {
        public Task<FileDTO> UploadFileAsync(FileDTO fileDTO);
    }
}
